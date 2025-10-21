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