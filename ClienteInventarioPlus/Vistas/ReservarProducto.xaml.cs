using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using Microsoft.VisualBasic; 
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace ClienteInventarioPlus.Vistas {
    public partial class ReservarProductoVista : UserControl {
        private Frame _mainFrame;
        private readonly IReservaService _proxyReserva;
        private readonly IProductoService _proxyProducto;
        public ObservableCollection<ProductoDTO>  Productos { get; set; } = new ObservableCollection<ProductoDTO>();

        public ReservarProductoVista(IReservaService proxyReserva, IProductoService proxyProducto, Frame frame) {
            InitializeComponent();
            _proxyReserva = proxyReserva;
            _proxyProducto = proxyProducto;
           
            _mainFrame = frame;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            CargarProductosDisponibles();
            CmbFiltro.SelectedIndex = 0;
        }

        private void CargarProductosDisponibles() {
            try {
                // Idealmente, tu servicio tendría un método que solo devuelve productos con stock > 0
                var productos = _proxyProducto.ObtenerProductos().Where(p => p.Stock > 0).ToList();
                DgProductos.ItemsSource = productos;
            }
            catch (Exception ex) {
                MessageBox.Show($"Error al cargar los productos: {ex.Message}", "Error de Servicio", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e) {
            // Lógica de búsqueda (similar a las otras vistas de consulta)
        }

        private void DgProductos_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // Habilita el botón de seleccionar solo si hay un producto elegido
            BtnSeleccionar.IsEnabled = DgProductos.SelectedItem != null;
        }

        // En ReservarProductoVista.xaml.cs

        private void BtnSeleccionar_Click(object sender, RoutedEventArgs e) {
            var productoSeleccionado = DgProductos.SelectedItem as ProductoDTO;
            if (productoSeleccionado == null) return;

            // Navega a la vista de detalles, pasando el producto seleccionado y el proxy
            _mainFrame.Content = new ProductoAReservarVista(productoSeleccionado, _proxyReserva, _proxyProducto,_mainFrame);
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) {
            // Regresa a la pantalla principal de reservas
            _mainFrame.Content = new ReservaPrincipal(_mainFrame,_proxyReserva, _proxyProducto);
        }
        
    }
}