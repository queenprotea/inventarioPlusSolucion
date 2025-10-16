using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace ClienteInventarioPlus.Vistas {
    public partial class ConsultarProductoEmpleado : UserControl {
        private readonly IProductoService _proxyProducto;

        public ConsultarProductoEmpleado(IProductoService proxyProducto) {
            InitializeComponent();
            _proxyProducto = proxyProducto;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            // Carga todos los productos cuando la vista se abre por primera vez
            CargarProductos();
            CmbFiltro.SelectedIndex = 0; // Selecciona "Buscar por Nombre" por defecto
        }

        private void CargarProductos(List<ProductoDTO> productos = null) {
            try {
                // Si no se pasa una lista específica, carga todos los productos
                if (productos == null) {
                    productos = _proxyProducto.ObtenerProductos();
                }
                DgProductos.ItemsSource = productos;
            }
            catch (Exception ex) {
                MessageBox.Show($"Error al cargar los productos: {ex.Message}", "Error de Servicio", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e) {
            string textoBusqueda = TxtBusqueda.Text;
            if (string.IsNullOrWhiteSpace(textoBusqueda)) {
                // Si la búsqueda está vacía, muestra todos los productos de nuevo
                CargarProductos();
                return;
            }

            try {
                List<ProductoDTO> productosFiltrados = new List<ProductoDTO>();
                string filtroSeleccionado = (CmbFiltro.SelectedItem as ComboBoxItem)?.Content.ToString();

                switch (filtroSeleccionado) {
                    case "Buscar por Nombre":
                        productosFiltrados = _proxyProducto.BuscarProductosPorNombre(textoBusqueda);
                        break;
                    case "Buscar por Código":
                        productosFiltrados = _proxyProducto.BuscarProductosPorCodigo(textoBusqueda);
                        break;
                    case "Buscar por Categoría":
                        productosFiltrados = _proxyProducto.BuscarProductosPorCategoria(textoBusqueda);
                        break;
                }

                DgProductos.ItemsSource = productosFiltrados;
            }
            catch (Exception ex) {
                MessageBox.Show($"Error al realizar la búsqueda: {ex.Message}", "Error de Servicio", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}