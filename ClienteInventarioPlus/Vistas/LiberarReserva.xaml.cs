using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;

namespace ClienteInventarioPlus.Vistas
{
    public partial class LiberarReserva : UserControl
    {
        private Frame _mainFrame;
        private IReservaService _proxy;
        private IProductoService _proxyProducto;
        public ObservableCollection<ReservaDTO> reservas { get; set; } = new ObservableCollection<ReservaDTO>();

        
        public LiberarReserva(Frame mainFrame, IReservaService proxy, IProductoService proxyProducto)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _proxy = proxy;
            _proxyProducto = proxyProducto;
            
            this.DataContext = this; // importante para el binding
            CargarReservas();
        }
        
        private void CargarReservas()
        {
            try
            {
                var lista = _proxy.ObtenerReservas();
                reservas.Clear();
                foreach (var u in lista)
                    reservas.Add(u);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar movimientos: {ex.Message}");
            }
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Content = new ReservaPrincipal(_mainFrame, _proxy, _proxyProducto);
        } 

        private void btnLiberarReserva_Click(object sender, RoutedEventArgs e)
        {
            var reserva = dgReservas.SelectedItem as ReservaDTO;

            if (reserva == null)
            {
                MessageBox.Show("Seleccione la reserva en la tabla para liberar.");
                return;
            }
              
            var confirmacion = MessageBox.Show(
                "¿Está seguro que desea liberar esta reserva?\n\nEsta acción no se puede deshacer.\n\nReservaID: "+reserva.ReservaID+"\n\n Numero de reserva: "+reserva.NumeroReserva+"\n\n ID del producto: "+reserva.ProductoID+
                "\n\n Cantidad Reservada: "+reserva.CantidadReservada+"\n\n Fecha y hora: "+reserva.FechaHora,
                "Confirmar anulación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (confirmacion != MessageBoxResult.Yes)
            {
                // Si el usuario cancela, no hace nada
                MessageBox.Show("Liberacion cancelada", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            if (_proxy == null)
            { 
                MessageBox.Show("El servicio no está disponible. Inténtelo más tarde."); 
                return;
            }
                
            bool liberacionReserva = _proxy.CancelarReserva(reserva.ReservaID); 

            if (liberacionReserva)
            {
                MessageBox.Show("Reservacion liberada con exito.");
                CargarReservas();
            }
            else
            {
                MessageBox.Show("Error al liberar la reserva");
            }
        }
    }
}