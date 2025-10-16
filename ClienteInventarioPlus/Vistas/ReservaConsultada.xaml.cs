using System;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace ClienteInventarioPlus.Vistas {
    public partial class ReservaDetalleVista : UserControl {
        private readonly IReservaService _proxyReserva;
        private readonly ReservaDTO _reserva;
        private Frame _mainFrame; // Necesitamos el frame para navegar hacia atrás

        public ReservaDetalleVista(Frame mainFrame, ReservaDTO reserva, IReservaService proxyReserva) {
            InitializeComponent();
            _mainFrame = mainFrame;
            _reserva = reserva;
            _proxyReserva = proxyReserva;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            if (_reserva != null) {
                TbReservaId.Text = _reserva.ReservaID.ToString();
                TbProducto.Text = _reserva.ProductoID.ToString();
                TbCantidad.Text = _reserva.CantidadReservada.ToString();
                TbFecha.Text = _reserva.FechaHora.ToString("g");
            }
        }

        private void BtnLiberar_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult confirmacion = MessageBox.Show(
                "¿Está seguro de que desea cancelar esta reserva?",
                "Confirmar Cancelación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirmacion == MessageBoxResult.Yes) {
                try {
                    bool resultado = _proxyReserva.CancelarReserva(_reserva.ReservaID);
                    if (resultado) {
                        MessageBox.Show("Reserva cancelada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        var nav = System.Windows.Navigation.NavigationService.GetNavigationService(this);
                        nav?.GoBack();
                    }
                    else {
                        MessageBox.Show("No se pudo cancelar la reserva.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show($"Error al procesar la cancelación: {ex.Message}", "Error de Servicio", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) {
            // Regresa a la pantalla anterior (la lista de reservas)
            var nav = System.Windows.Navigation.NavigationService.GetNavigationService(this);
            nav?.GoBack();
        }
    }
}