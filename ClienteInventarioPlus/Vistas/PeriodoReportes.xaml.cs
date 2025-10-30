using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using ClienteInventarioPlus.Utilidades;
using FastReport;
using FastReport.Utils;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;
using Border = FastReport.Border;
using Path = System.IO.Path;

namespace ClienteInventarioPlus.Vistas {
    public partial class SeleccionarPeriodoVista : UserControl {
        private Frame _mainFrame;
        private IProductoService _proxyProducto;
        private IMovimientoService _proxyMovimiento;
        private UsuarioDTO usuarioActual;
        private ObservableCollection<MovimientoDTO> _movimientos;

        // El constructor recibe el tipo de reporte para saber cuál generar
        public SeleccionarPeriodoVista(Frame mainFrame , IProductoService proxyProducto, IMovimientoService proxyMovimiento, UsuarioDTO usuarioSesion) {
            InitializeComponent();
            _mainFrame = mainFrame;
            _proxyProducto = proxyProducto;
            _proxyMovimiento = proxyMovimiento;
            usuarioActual = usuarioSesion;
        }

        private void BtnGenerar_Click(object sender, RoutedEventArgs e) {
            // --- 1. Validar Fechas ---
            
            string mensajeErrorInicio = ValidacionesEntrada.ValidarFecha(DpFechaInicio);
            string mensajeErrorFin = ValidacionesEntrada.ValidarFecha(DpFechaFin);

            if (mensajeErrorFin != null)
            {
                MessageBox.Show(mensajeErrorFin);
                return;
            }
            
            if (mensajeErrorInicio != null)
            {
                MessageBox.Show(mensajeErrorInicio);
                return;
            }
            
            if (DpFechaInicio.SelectedDate == null || DpFechaFin.SelectedDate == null) {
                MessageBox.Show("Debe seleccionar una fecha de inicio y una fecha final.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime fechaInicio = DpFechaInicio.SelectedDate.Value;
            DateTime fechaFin = DpFechaFin.SelectedDate.Value.Date.AddDays(1).AddTicks(-1); // hasta 23:59:59.9999999

            if (fechaFin < fechaInicio) {
                MessageBox.Show("La fecha final no puede ser anterior a la fecha inicial.", "Fechas Inválidas", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // --- 2. Llamar al metodo y navegar a la Vista Previa ---
            try
            {
                string rutaMovimiento = GenerarReporteMovimientos(fechaInicio, fechaFin);
                _mainFrame.Content = new ReportePreviaVista(_mainFrame, rutaMovimiento,"ReporteMovimientos");
            }
            catch (Exception ex) {
                MessageBox.Show($"Ocurrió un error al generar el reporte: {ex.Message}", "Error de Servicio", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private string GenerarReporteMovimientos(DateTime fechaInicio, DateTime fechaFin)
        {
            // Crear DataTable con los datos del servicio
            DataTable movimientos = new DataTable("Movimientos");
            movimientos.Columns.Add("ID", typeof(int));
            movimientos.Columns.Add("ProductoDisplay", typeof(string));
            movimientos.Columns.Add("TipoMovimiento", typeof(string));
            movimientos.Columns.Add("Cantidad", typeof(int));
            movimientos.Columns.Add("FechaHora", typeof(DateTime));
            movimientos.Columns.Add("UsuarioDisplay", typeof(string));

            _movimientos = new ObservableCollection<MovimientoDTO>(_proxyMovimiento.ObtenerMovimientosPorRangoFecha(fechaInicio, fechaFin).OrderBy(m => m.FechaHora));

            foreach (MovimientoDTO movimiento in _movimientos)
            {
                movimientos.Rows.Add(
                    movimiento.MovimientoID,
                    movimiento.ProductoDisplay,
                    movimiento.TipoMovimiento,
                    movimiento.Cantidad,
                    movimiento.FechaHora,
                    movimiento.UsuarioDisplay
                );
            }

            // Crear el reporte
            using (Report reporte = new Report())
            {
                reporte.RegisterData(movimientos, "Movimientos");
                (reporte.GetDataSource("Movimientos") as FastReport.Data.DataSourceBase).Enabled = true;

                // Crear página
                ReportPage pagina = new ReportPage();
                reporte.Pages.Add(pagina);

                // --- Título ---
                ReportTitleBand titulo = new ReportTitleBand();
                titulo.Height = Units.Centimeters * 5f; // más alto para no interferir
                pagina.ReportTitle = titulo;

                TextObject txtTitulo = new TextObject();
                txtTitulo.Bounds = new System.Drawing.RectangleF(0, 0, Units.Centimeters * 19, Units.Centimeters * 1);
                txtTitulo.Text = "REPORTE DE MOVIMIENTOS DE INVENTARIO";
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
                    },
                    new TextObject()
                    {
                        Bounds = new System.Drawing.RectangleF(0, yBase + Units.Centimeters * 1.2f, Units.Centimeters * 19, Units.Centimeters * 0.6f),
                        Text = $"Periodo del reporte: {fechaInicio:dd/MM/yyyy HH:mm} - {fechaFin:dd/MM/yyyy HH:mm}",
                        Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold),
                        HorzAlign = HorzAlign.Center
                    }
                });

                // --- Encabezados dentro del ReportTitleBand
                string[] headers = { "ID", "Producto", "Tipo de movimiento", "Cantidad", "FechaHora", "Usuario" };
                float[] posiciones = { 0, 1, 7, 9.2f, 11.2f, 16.2f};
                float alturaEncabezado = Units.Centimeters * 0.8f;
                float yEncabezado = yBase + Units.Centimeters * 2.8f; // justo debajo de los datos de empresa/usuario

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
                datos.DataSource = reporte.GetDataSource("Movimientos");
                datos.CanGrow = true;
                datos.CanShrink = true;
                datos.Height = Units.Centimeters * 1;
                pagina.Bands.Add(datos);

                string[] campos = { "ID", "ProductoDisplay", "TipoMovimiento", "Cantidad", "FechaHora", "UsuarioDisplay" };

                for (int i = 0; i < campos.Length; i++)
                {
                    datos.Objects.Add(new TextObject()
                    {
                        Bounds = new System.Drawing.RectangleF(
                            posiciones[i] * Units.Centimeters,
                            0,
                            (i < campos.Length - 1 ? (posiciones[i + 1] - posiciones[i]) : 2) * Units.Centimeters,
                            Units.Centimeters * 1),
                        Text = $"[Movimientos.{campos[i]}]",
                        Border = new Border() { Lines = BorderLines.All },
                        HorzAlign = HorzAlign.Center,
                        VertAlign = VertAlign.Center
                    });
                }


                // --- Exportar a PDF ---
                reporte.Prepare();

                string carpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReportesGenerados");
                Directory.CreateDirectory(carpeta);
                string ruta = Path.Combine(carpeta, "ReporteMovimientos.pdf");

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

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) {
            if (_mainFrame.NavigationService.CanGoBack) {
                _mainFrame.NavigationService.GoBack();
            }
        }
    }
}
