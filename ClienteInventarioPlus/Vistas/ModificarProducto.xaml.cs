using System;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace ClienteInventarioPlus.Vistas {
    public partial class ModificarProductoVista : UserControl {
        private readonly IProductoService _proxyProducto;
        private readonly ProductoDTO _productoAModificar; // Almacena el producto que estamos editando

        public ModificarProductoVista(ProductoDTO producto, IProductoService proxyProducto) {
            InitializeComponent();
            _productoAModificar = producto;
            _proxyProducto = proxyProducto;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            // Cargar los datos del producto en los campos del formulario
            CargarDatosDelProducto();

            // También puedes cargar las listas de categorías y proveedores aquí
            // CargarCategorias();
            // CargarProveedores();
        }

        private void CargarDatosDelProducto() {
            // Rellena cada campo con la información del producto recibido
            TxtNombreProducto.Text = _productoAModificar.Nombre;
            TxtCodigo.Text = _productoAModificar.Codigo; // Ajustado al DTO real
            TxtDescripcion.Text = _productoAModificar.Descripcion;
            TxtStockActual.Text = _productoAModificar.Stock.ToString();
            TxtStockMinimo.Text = _productoAModificar.StockMinimo.ToString();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e) {
            // --- 1. Validar Entradas ---
            if (string.IsNullOrWhiteSpace(TxtNombreProducto.Text)) {
                MessageBox.Show("El nombre del producto no puede estar vacío.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // --- 2. Actualizar el Objeto DTO con los nuevos valores ---
            try {
                _productoAModificar.Nombre = TxtNombreProducto.Text;
                _productoAModificar.Codigo = TxtCodigo.Text;
                _productoAModificar.Descripcion = TxtDescripcion.Text;
                _productoAModificar.Stock = int.Parse(TxtStockActual.Text);
                _productoAModificar.StockMinimo = int.Parse(TxtStockMinimo.Text);
            }
            catch (FormatException) {
                MessageBox.Show("Por favor, asegúrese de que los campos numéricos (stock) sean correctos.", "Error de Formato", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // --- 3. Llamar al Servicio para Actualizar ---
            try {
                bool resultado = _proxyProducto.ActualizarProducto(_productoAModificar);
                if (resultado) {
                    MessageBox.Show("Producto actualizado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Regresar a la vista anterior de forma segura
                    var nav = System.Windows.Navigation.NavigationService.GetNavigationService(this);
                    nav?.GoBack();
                }
                else {
                    MessageBox.Show("No se pudo actualizar el producto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Error al conectar con el servicio: {ex.Message}", "Error de Servicio", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) {
            // Regresar a la pantalla anterior (la lista de productos)
            var nav = System.Windows.Navigation.NavigationService.GetNavigationService(this);
            nav?.GoBack();
        }
    }
}
