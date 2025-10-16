using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos; // Asegúrate de tener la referencia

namespace ClienteInventarioPlus.Vistas {
    public partial class ReservaPrincipal : UserControl {
        private readonly Frame _mainFrame;
        private readonly IReservaService _proxyReserva;

        public ReservaPrincipal(Frame mainFrame, IReservaService proxyReserva) {
            InitializeComponent();
            _mainFrame = mainFrame;
            _proxyReserva = proxyReserva;
        }

        private void btnReservar_Click(object sender, RoutedEventArgs e) {
            // Pendiente de implementación cuando el servicio de productos esté disponible.
            MessageBox.Show("La vista 'Reservar Producto' estará disponible cuando se termine la integración del servicio.");
        }

        private void btnLiberar_Click(object sender, RoutedEventArgs e) {
            // Pendiente de implementación (la vista LiberarReservaVista no existe aún).
            MessageBox.Show("La funcionalidad de liberar/cancelar reservas estará disponible próximamente.");
        }
    }
}