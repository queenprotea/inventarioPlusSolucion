using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace ClienteInventarioPlus.Vistas {
    public partial class ConsultarProductoAdmin : UserControl {
        private readonly IProductoService _proxyProducto;

        public ConsultarProductoAdmin(IProductoService proxyProducto) {
            InitializeComponent();
            _proxyProducto = proxyProducto;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            CargarProductos();
            CmbFiltro.SelectedIndex = 0;
        }

        private void CargarProductos() {
            try {
                var productos = _proxyProducto.ObtenerProductos();
                DgProductos.ItemsSource = productos;
            }
            catch (Exception ex) {
                MessageBox.Show($"Error al cargar los productos: {ex.Message}", "Error de Servicio", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e) {
            // La lógica de búsqueda es la misma que en la vista de empleado
            // ... (puedes copiarla de la respuesta anterior)
        }

        private void DgProductos_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // Habilita los botones Modificar y Eliminar solo si hay un producto seleccionado
            bool haySeleccion = DgProductos.SelectedItem != null;
            BtnModificar.IsEnabled = haySeleccion;
            BtnEliminar.IsEnabled = haySeleccion;
        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e) {
            // Obtiene el producto seleccionado de la tabla
            var productoSeleccionado = DgProductos.SelectedItem as ProductoDTO;
            if (productoSeleccionado == null) return;

            // Navega a la nueva vista de modificación, pasándole el producto y el proxy de forma segura
            var nav = System.Windows.Navigation.NavigationService.GetNavigationService(this);
            nav?.Navigate(new ModificarProductoVista(productoSeleccionado, _proxyProducto));
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
    }
}
