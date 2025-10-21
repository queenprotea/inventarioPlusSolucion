using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace ClienteInventarioPlus.Vistas
{
    public partial class ConsultarProductoAdmin : UserControl
    {
        private readonly IProductoService _proxyProducto;
        private readonly IProveedorService _proxyProveedor;
        private Frame _mainFrame;
        private ObservableCollection<ProductoDTO> _productos;

        public ConsultarProductoAdmin(IProductoService proxyProducto, IProveedorService proxyProveedor, Frame mainFrame)
        {
            InitializeComponent();
            _proxyProducto = proxyProducto;
            _proxyProveedor = proxyProveedor;
            _mainFrame = mainFrame;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CargarProductos();
            CmbFiltro.SelectedIndex = 0;
        }

        private void CargarProductos()
        {
            try
            {
                _productos = new ObservableCollection<ProductoDTO>(_proxyProducto.ObtenerProductos());
                DgProductos.ItemsSource = _productos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los productos: {ex.Message}", "Error de Servicio",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string textoBusqueda = TxtBusqueda.Text.Trim();
            if (string.IsNullOrWhiteSpace(textoBusqueda))
            {
                DgProductos.ItemsSource = _proxyProducto.ObtenerProductos();
                MessageBox.Show("Por favor, ingresa un texto para buscar.",
                    "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Obtener el criterio seleccionado
            var itemSeleccionado = (ComboBoxItem)CmbFiltro.SelectedItem;
            string criterio = itemSeleccionado.Content.ToString();

            List<ProductoDTO> resultado = new List<ProductoDTO>();

            // Filtrar según el criterio seleccionado
            switch (criterio)
            {
                case "Buscar por Nombre":
                    resultado = _proxyProducto.ObtenerProductos()
                        .Where(p => p.Nombre.Contains(textoBusqueda))
                        .ToList();
                    break;

                case "Buscar por Código":
                    resultado = _proxyProducto.ObtenerProductos()
                        .Where(p => p.Codigo.Contains(textoBusqueda))
                        .ToList();
                    break;

                case "Buscar por Categoría":
                    resultado = _proxyProducto.ObtenerProductos()
                        .Where(p => p.NombreCategoria != null &&
                                    p.NombreCategoria.Contains(textoBusqueda))
                        .ToList();
                    break;

                default:
                    MessageBox.Show("Selecciona un criterio de búsqueda válido.");
                    return;
            }

            // Mostrar los resultados en el DataGrid
            _productos = new ObservableCollection<ProductoDTO>(resultado);
            DgProductos.ItemsSource = _productos;
        }
        
        private void DgProductos_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // Habilita los botones Modificar y Eliminar solo si hay un producto seleccionado
            bool haySeleccion = DgProductos.SelectedItem != null;
            BtnModificar.IsEnabled = haySeleccion;
            BtnEliminar.IsEnabled = haySeleccion;
            BtnConusltar.IsEnabled = haySeleccion;
        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e) {
            // Obtiene el producto seleccionado de la tabla
            var productoSeleccionado = DgProductos.SelectedItem as ProductoDTO;
            if (productoSeleccionado == null) return;

            // Navega a la nueva vista de modificación, pasándole el producto y el proxy de forma segura
            _mainFrame.Content = new ModificarProductoVista(productoSeleccionado, _proxyProducto, _proxyProveedor, "modificar");
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e) {
            var productoSeleccionado = DgProductos.SelectedItem as ProductoDTO;
            if (productoSeleccionado == null) return;

            // Pedir confirmación antes de eliminar
            var confirmacion = MessageBox.Show(
                $"¿Está seguro de que desea eliminar el producto '{productoSeleccionado.Nombre}'?\nEsta acción no se puede deshacer.",
                "Confirmar Eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmacion == MessageBoxResult.Yes) {
                try {
                    // Ajustar el nombre de la propiedad de ID al DTO real
                    bool resultado = _proxyProducto.EliminarProducto(productoSeleccionado.ProductoID);
                    if (resultado) {
                        MessageBox.Show("Producto eliminado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        // Vuelve a cargar la lista para que el producto eliminado ya no aparezca
                        CargarProductos();
                    }
                    else {
                        MessageBox.Show("No se pudo eliminar el producto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show($"Error al eliminar el producto: {ex.Message}", "Error de Servicio", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Content = new ModificarProductoVista(DgProductos.SelectedItem as ProductoDTO, _proxyProducto, _proxyProveedor, "consultar");
        }
    }
}
