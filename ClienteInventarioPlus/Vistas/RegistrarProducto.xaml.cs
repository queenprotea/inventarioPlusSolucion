using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox; // O el que estés usando

namespace ClienteInventarioPlus.Vistas {
    public partial class RegistrarProductoVista : UserControl {
        // Proxies para los servicios que esta vista necesita
        private readonly IProductoService _proxyProducto;
        private readonly IProveedorService _proxyProveedor;
        // private readonly ICategoriaService _proxyCategoria; // Descomenta si tienes servicio de categorías

        public RegistrarProductoVista(IProductoService proxyProducto, IProveedorService proxyProveedor) {
            InitializeComponent();
            _proxyProducto = proxyProducto;
            _proxyProveedor = proxyProveedor;
            // _proxyCategoria = proxyCategoria;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            // Este método se ejecuta cuando la vista se carga.
            // Es ideal para poblar ComboBoxes y DataGrids.
            CargarProveedores();
            // CargarCategorias(); // Descomenta cuando tengas el servicio
        }

        private void CargarProveedores() {
            try {
                // Llama al servicio para obtener la lista de todos los proveedores
                var proveedores = _proxyProveedor.ObtenerProveedores();
                DgProveedores.ItemsSource = proveedores;
            }
            catch (Exception ex) {
                MessageBox.Show($"Error al cargar los proveedores: {ex.Message}", "Error de Servicio", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*
        private void CargarCategorias()
        {
            try
            {
                // Llama al servicio para obtener las categorías
                var categorias = _proxyCategoria.ObtenerCategorias();
                CmbCategoria.ItemsSource = categorias;
                CmbCategoria.DisplayMemberPath = "Nombre"; // La propiedad del objeto a mostrar
                CmbCategoria.SelectedValuePath = "IdCategoria"; // El valor a usar internamente
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las categorías: {ex.Message}", "Error de Servicio", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        */

        private void BtnGuardar_Click(object sender, RoutedEventArgs e) {
            // --- 1. Validar Entradas ---
            if (string.IsNullOrWhiteSpace(TxtNombreProducto.Text) ||
                string.IsNullOrWhiteSpace(TxtPrecioCompra.Text) ||
                string.IsNullOrWhiteSpace(TxtPrecioVenta.Text) ||
                string.IsNullOrWhiteSpace(TxtStockInicial.Text)) {
                MessageBox.Show("Por favor, complete todos los campos obligatorios.", "Campos Vacíos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // --- 2. Crear el Objeto DTO ---
            var nuevoProducto = new ProductoDTO();

            try {
                nuevoProducto.Nombre = TxtNombreProducto.Text;
                nuevoProducto.Descripcion = TxtDescripcion.Text;
                nuevoProducto.Codigo = TxtCodigo.Text; // Ajustado al DTO real

                if (!int.TryParse(TxtStockInicial.Text, out int stockInicial)) {
                    MessageBox.Show("El stock inicial no es un número entero válido.", "Error de Formato", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                nuevoProducto.Stock = stockInicial;

                // El stock mínimo es opcional, si está vacío se puede poner 0.
                int.TryParse(TxtStockMinimo.Text, out int stockMinimo);
                nuevoProducto.StockMinimo = stockMinimo;

                // nuevoProducto.IdCategoria = (int)CmbCategoria.SelectedValue; // Ejemplo
            }
            catch (Exception ex) {
                MessageBox.Show($"Error al preparar los datos del producto: {ex.Message}", "Error de Datos", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // --- 3. Llamar al Servicio ---
            try {
                bool resultado = _proxyProducto.RegistrarProducto(nuevoProducto);
                if (resultado) {
                    MessageBox.Show("Producto registrado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarFormulario();
                }
                else {
                    MessageBox.Show("No se pudo registrar el producto. Verifique los datos o intente más tarde.", "Fallo el Registro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Error al conectar con el servicio: {ex.Message}", "Error de Servicio", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) {
            LimpiarFormulario();
        }

        private void LimpiarFormulario() {
            TxtNombreProducto.Clear();
            TxtPrecioCompra.Clear();
            TxtPrecioVenta.Clear();
            TxtCodigo.Clear();
            TxtDescripcion.Clear();
            TxtStockInicial.Clear();
            TxtStockMinimo.Clear();
            CmbCategoria.SelectedIndex = -1; // Deselecciona cualquier ítem
        }
    }
}