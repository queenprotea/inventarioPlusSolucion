using System;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos; // Asegúrate de tener la referencia

namespace ClienteInventarioPlus.Vistas {
    public partial class ReservaPrincipal : UserControl {
        private readonly Frame _mainFrame;
        private readonly IReservaService _proxyReserva;
        private readonly IProductoService _proxyProducto;

        public ReservaPrincipal(Frame mainFrame, IReservaService proxyReserva, IProductoService proxyProducto) {
            InitializeComponent();
            _mainFrame = mainFrame;
            _proxyReserva = proxyReserva;
            _proxyProducto = proxyProducto;
            
            MostrarProductosStockBajo();
        }

        private void MostrarProductosStockBajo()
        {
            try
            {
                // Llamamos al método que devuelve los productos con stock bajo
                var lista = _proxyProducto.ObtenerProductosConStockBajo();

                if (lista.Count > 0)
                {
                    // Construimos el mensaje con los nombres y stock de los productos
                    string mensaje = "Productos con stock bajo:\n\n";
                    foreach (var prod in lista)
                    {
                        mensaje += $"({prod.Codigo}) {prod.Nombre} - Stock: {prod.Stock} (Stock mínimo: {prod.StockMinimo})\n";
                    }

                    MessageBox.Show(
                        mensaje,
                        "Productos con stock bajo",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}");
            }
        }
        
        private void btnReservar_Click(object sender, RoutedEventArgs e) {
            // Pendiente de implementación cuando el servicio de productos esté disponible.
            _mainFrame.Content = new ReservarProductoVista(_proxyReserva, _proxyProducto, _mainFrame);
        }

        private void btnLiberar_Click(object sender, RoutedEventArgs e) {
            // Pendiente de implementación (la vista LiberarReservaVista no existe aún).
            _mainFrame.Content = new LiberarReserva(_mainFrame, _proxyReserva, _proxyProducto);
        }
    }
}