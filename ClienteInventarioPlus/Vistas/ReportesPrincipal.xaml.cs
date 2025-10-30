using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using FastReport;
using FastReport.Utils;
using Border = FastReport.Border;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;
using FastReport.Export.PdfSimple;
using System.IO;
using FastReport.Table;
using Path = System.IO.Path;


namespace ClienteInventarioPlus.Vistas {
    public partial class ReportesPrincipal : UserControl {
        private IProductoService _proxyProducto;
        private IMovimientoService _proxyMovimiento;
        private Frame _mainFrame;
        private ObservableCollection<ProductoDTO> _productos;
        private UsuarioDTO usuarioActual;

        public ReportesPrincipal(Frame mainFrame, IProductoService proxyProducto, IMovimientoService proxyMovimiento, UsuarioDTO usuarioSesion) {
            InitializeComponent();
            _mainFrame = mainFrame;
            _proxyProducto = proxyProducto;
            _proxyMovimiento = proxyMovimiento;
            usuarioActual = usuarioSesion;
        }

        private void BtnReporteInventario_Click(object sender, RoutedEventArgs e) {
            string rutaInventario = GenerarReporteInventario();
            _mainFrame.Content = new ReportePreviaVista(_mainFrame, rutaInventario,"ReporteInventario");
        }
    
        
        private string GenerarReporteInventario()
        {
            // Crear DataTable con los datos del servicio
            DataTable productos = new DataTable("Productos");
            productos.Columns.Add("ID", typeof(int));
            productos.Columns.Add("Nombre", typeof(string));
            productos.Columns.Add("Categoria", typeof(string));
            productos.Columns.Add("PrecioCompra", typeof(decimal));
            productos.Columns.Add("PrecioVenta", typeof(decimal));
            productos.Columns.Add("Stock", typeof(int));
            productos.Columns.Add("StockApartado", typeof(int));

            _productos = new ObservableCollection<ProductoDTO>(_proxyProducto.ObtenerProductos());

            foreach (ProductoDTO producto in _productos)
            {
                productos.Rows.Add(
                    producto.ProductoID,
                    producto.Nombre,
                    producto.NombreCategoria,
                    producto.PrecioCompra,
                    producto.PrecioVenta,
                    producto.Stock,
                    producto.StockApartado
                );
            }

            // Crear el reporte
            using (Report reporte = new Report())
            {
                reporte.RegisterData(productos, "Productos");
                (reporte.GetDataSource("Productos") as FastReport.Data.DataSourceBase).Enabled = true;

                // Crear página
                ReportPage pagina = new ReportPage();
                reporte.Pages.Add(pagina);

                // --- Título ---
                ReportTitleBand titulo = new ReportTitleBand();
                titulo.Height = Units.Centimeters * 5f; // más alto para no interferir
                pagina.ReportTitle = titulo;

                TextObject txtTitulo = new TextObject();
                txtTitulo.Bounds = new System.Drawing.RectangleF(0, 0, Units.Centimeters * 19, Units.Centimeters * 1);
                txtTitulo.Text = "REPORTE DE INVENTARIO";
                txtTitulo.HorzAlign = HorzAlign.Center;
                txtTitulo.Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold);
                titulo.Objects.Add(txtTitulo);

                float yBase = Units.Centimeters * 1.4f;

                titulo.Objects.AddRange(new[]
                {
                    new TextObject()
                    {
                        Bounds = new System.Drawing.RectangleF(0, yBase, Units.Centimeters * 10, Units.Centimeters * 0.6f),
                        Text = "Empresa: Tienda Inventario Plus",
                        Font = new System.Drawing.Font("Arial", 10)
                    },
                    new TextObject()
                    {
                        Bounds = new System.Drawing.RectangleF(0, yBase + Units.Centimeters * 0.6f, Units.Centimeters * 10, Units.Centimeters * 0.6f),
                        Text = "Sucursal: Veracruz Centro",
                        Font = new System.Drawing.Font("Arial", 10)
                    },
                    new TextObject()
                    {
                        Bounds = new System.Drawing.RectangleF(11 * Units.Centimeters, yBase, Units.Centimeters * 8, Units.Centimeters * 0.6f),
                        Text = $"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}",
                        Font = new System.Drawing.Font("Arial", 10),
                        HorzAlign = HorzAlign.Right
                    },
                    new TextObject()
                    {
                        Bounds = new System.Drawing.RectangleF(11 * Units.Centimeters, yBase + Units.Centimeters * 0.6f, Units.Centimeters * 8, Units.Centimeters * 0.6f),
                        Text = $"Generado por: {usuarioActual.Nombre}",
                        Font = new System.Drawing.Font("Arial", 10),
                        HorzAlign = HorzAlign.Right
                    }
                });

                // --- Encabezados dentro del ReportTitleBand ---
                string[] headers = { "ID", "Nombre", "Categoría", "P.Compra", "P.Venta", "Stock", "Apartado" };
                float[] posiciones = { 0, 2, 9, 12, 14, 16, 17.5f };
                float alturaEncabezado = Units.Centimeters * 0.6f;
                float yEncabezado = yBase + Units.Centimeters * 3.0f; // justo debajo de los datos de empresa/usuario

                for (int i = 0; i < headers.Length; i++)
                {
                    TextObject txtHeader = new TextObject();
                    txtHeader.Bounds = new System.Drawing.RectangleF(
                        posiciones[i] * Units.Centimeters,
                        yEncabezado,
                        (i < headers.Length - 1 ? (posiciones[i + 1] - posiciones[i]) : 2) * Units.Centimeters,
                        alturaEncabezado
                    );
                    txtHeader.Text = headers[i];
                    txtHeader.Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold);
                    txtHeader.HorzAlign = HorzAlign.Center;
                    txtHeader.VertAlign = VertAlign.Center;
                    txtHeader.Border = new Border() { Lines = BorderLines.All, Color = System.Drawing.Color.Black };
                    titulo.Objects.Add(txtHeader);
                }



                // --- Datos (DataBand) ---
                DataBand datos = new DataBand();
                datos.Parent = pagina; // importante
                datos.DataSource = reporte.GetDataSource("Productos");
                datos.CanGrow = true;
                datos.CanShrink = true;
                datos.Height = Units.Centimeters * 1.0f;
                pagina.Bands.Add(datos);

                string[] campos = { "ID", "Nombre", "Categoria", "PrecioCompra", "PrecioVenta", "Stock", "StockApartado" };

                for (int i = 0; i < campos.Length; i++)
                {
                    datos.Objects.Add(new TextObject()
                    {
                        Bounds = new System.Drawing.RectangleF(
                            posiciones[i] * Units.Centimeters,
                            0,
                            (i < campos.Length - 1 ? (posiciones[i + 1] - posiciones[i]) : 2) * Units.Centimeters,
                            Units.Centimeters * 1.0f),
                        Text = $"[Productos.{campos[i]}]",
                        Border = new Border() { Lines = BorderLines.All },
                        HorzAlign = HorzAlign.Center,
                        VertAlign = VertAlign.Center
                    });
                }


                // --- Exportar a PDF ---
                reporte.Prepare();

                string carpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReportesGenerados");
                Directory.CreateDirectory(carpeta);
                string ruta = Path.Combine(carpeta, "ReporteInventario.pdf");

                try
                {
                    if (File.Exists(ruta))
                        File.Delete(ruta);
                }
                catch (IOException)
                {
                    MessageBox.Show("El reporte anterior aún está abierto. Cierre el visor antes de generar uno nuevo.");
                    return null;
                }

                using (var exportador = new FastReport.Export.PdfSimple.PDFSimpleExport())
                {
                    exportador.Export(reporte, ruta);
                }
                
                return ruta;
            }
        }
        

        private void BtnReporteMovimientos_Click(object sender, RoutedEventArgs e) {
            _mainFrame.Content = new SeleccionarPeriodoVista(_mainFrame, _proxyProducto, _proxyMovimiento, usuarioActual);
        }

        private void BtnListaProductosCriticos_Click(object sender, RoutedEventArgs e)
        {
            string rutaCriticos = GenerarListaProductosCriticos();
            _mainFrame.Content = new ReportePreviaVista(_mainFrame, rutaCriticos, "ReporteProductosCriticos");
        }

        private string GenerarListaProductosCriticos()
        {
            // Crear DataTable con los datos del servicio
            DataTable productos = new DataTable("Productos");
            productos.Columns.Add("ID", typeof(int));
            productos.Columns.Add("Nombre", typeof(string));
            productos.Columns.Add("Categoria", typeof(string));
            productos.Columns.Add("Stock", typeof(int));
            productos.Columns.Add("StockApartado", typeof(int));
            productos.Columns.Add("StockMinimo", typeof(int));
            productos.Columns.Add("FirstProveedor", typeof(string));
            

        _productos = new ObservableCollection<ProductoDTO>(_proxyProducto.ObtenerProductosConStockBajo());

            foreach (ProductoDTO producto in _productos)
            {
                productos.Rows.Add(
                    producto.ProductoID,
                    producto.Nombre,
                    producto.NombreCategoria,
                    producto.Stock,
                    producto.StockApartado,
                    producto.StockMinimo,
                    producto.FirstProveedor ?? "N/A"
                );
            }

            // Crear el reporte
            using (Report reporte = new Report())
            {
                reporte.RegisterData(productos, "Productos");
                (reporte.GetDataSource("Productos") as FastReport.Data.DataSourceBase).Enabled = true;

                // Crear página
                ReportPage pagina = new ReportPage();
                reporte.Pages.Add(pagina);

                // --- Título ---
                ReportTitleBand titulo = new ReportTitleBand();
                titulo.Height = Units.Centimeters * 5f; // más alto para no interferir
                pagina.ReportTitle = titulo;

                TextObject txtTitulo = new TextObject();
                txtTitulo.Bounds = new System.Drawing.RectangleF(0, 0, Units.Centimeters * 19, Units.Centimeters * 1);
                txtTitulo.Text = "PRODUCTOS CRITICOS";
                txtTitulo.HorzAlign = HorzAlign.Center;
                txtTitulo.Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold);
                titulo.Objects.Add(txtTitulo);

                float yBase = Units.Centimeters * 1.4f;

                titulo.Objects.AddRange(new[]
                {
                    new TextObject()
                    {
                        Bounds = new System.Drawing.RectangleF(0, yBase, Units.Centimeters * 10, Units.Centimeters * 0.6f),
                        Text = "Empresa: Tienda Inventario Plus",
                        Font = new System.Drawing.Font("Arial", 10)
                    },
                    new TextObject()
                    {
                        Bounds = new System.Drawing.RectangleF(0, yBase + Units.Centimeters * 0.6f, Units.Centimeters * 10, Units.Centimeters * 0.6f),
                        Text = "Sucursal: Veracruz Centro",
                        Font = new System.Drawing.Font("Arial", 10)
                    },
                    new TextObject()
                    {
                        Bounds = new System.Drawing.RectangleF(11 * Units.Centimeters, yBase, Units.Centimeters * 8, Units.Centimeters * 0.6f),
                        Text = $"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}",
                        Font = new System.Drawing.Font("Arial", 10),
                        HorzAlign = HorzAlign.Right
                    },
                    new TextObject()
                    {
                        Bounds = new System.Drawing.RectangleF(11 * Units.Centimeters, yBase + Units.Centimeters * 0.6f, Units.Centimeters * 8, Units.Centimeters * 0.6f),
                        Text = $"Generado por: {usuarioActual.Nombre}",
                        Font = new System.Drawing.Font("Arial", 10),
                        HorzAlign = HorzAlign.Right
                    }
                });

                // --- Encabezados dentro del ReportTitleBand ---
                string[] headers = { "ID", "Nombre", "Categoría", "Stock", "Apartado", "Stock minimo", "Proveedor" };
                float[] posiciones = { 0, 2, 9, 12, 14, 16, 17.5f };
                float alturaEncabezado = Units.Centimeters * 0.6f;
                float yEncabezado = yBase + Units.Centimeters * 3.0f; // justo debajo de los datos de empresa/usuario

                for (int i = 0; i < headers.Length; i++)
                {
                    TextObject txtHeader = new TextObject();
                    txtHeader.Bounds = new System.Drawing.RectangleF(
                        posiciones[i] * Units.Centimeters,
                        yEncabezado,
                        (i < headers.Length - 1 ? (posiciones[i + 1] - posiciones[i]) : 2) * Units.Centimeters,
                        alturaEncabezado
                    );
                    txtHeader.Text = headers[i];
                    txtHeader.Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold);
                    txtHeader.HorzAlign = HorzAlign.Center;
                    txtHeader.VertAlign = VertAlign.Center;
                    txtHeader.Border = new Border() { Lines = BorderLines.All, Color = System.Drawing.Color.Black };
                    titulo.Objects.Add(txtHeader);
                }



                // --- Datos (DataBand) ---
                DataBand datos = new DataBand();
                datos.Parent = pagina; // importante
                datos.DataSource = reporte.GetDataSource("Productos");
                datos.CanGrow = true;
                datos.CanShrink = true;
                datos.Height = Units.Centimeters * 1.0f;
                pagina.Bands.Add(datos);

                string[] campos = { "ID", "Nombre", "Categoria", "Stock", "StockApartado", "StockMinimo", "FirstProveedor" };

                for (int i = 0; i < campos.Length; i++)
                {
                    datos.Objects.Add(new TextObject()
                    {
                        Bounds = new System.Drawing.RectangleF(
                            posiciones[i] * Units.Centimeters,
                            0,
                            (i < campos.Length - 1 ? (posiciones[i + 1] - posiciones[i]) : 2) * Units.Centimeters,
                            Units.Centimeters * 1.0f),
                        Text = $"[Productos.{campos[i]}]",
                        Border = new Border() { Lines = BorderLines.All },
                        HorzAlign = HorzAlign.Center,
                        VertAlign = VertAlign.Center
                    });
                }


                // --- Exportar a PDF ---
                reporte.Prepare();

                string carpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReportesGenerados");
                Directory.CreateDirectory(carpeta);
                string ruta = Path.Combine(carpeta, "ReporteProductosCriticos.pdf");

                try
                {
                    if (File.Exists(ruta))
                        File.Delete(ruta);
                }
                catch (IOException)
                {
                    MessageBox.Show("El reporte anterior aún está abierto. Cierre el visor antes de generar uno nuevo.");
                    return null;
                }

                using (var exportador = new FastReport.Export.PdfSimple.PDFSimpleExport())
                {
                    exportador.Export(reporte, ruta);
                }
                
                return ruta;
            }
        }
        
    }
}