using System;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;
using System.Collections.Generic;
using BibliotecaClasesNetframework.ModelosODT;

namespace ClienteInventarioPlus.Vistas {
    public partial class ConsultarReservasVista : UserControl {
        private readonly IReservaService _proxyReserva;
        private readonly IProductoService _proxyProducto;
        private Frame _mainFrame;

        public ConsultarReservasVista(Frame mainFrame, IReservaService proxyReserva, IProductoService  proxyProducto ) {
            InitializeComponent();
            _mainFrame = mainFrame;
            _proxyReserva = proxyReserva;
            _proxyProducto = proxyProducto;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            CargarReservasActivas();
            CmbFiltro.SelectedIndex = 0;
        }

        private void CargarReservasActivas(List<ReservaDTO> reservas = null) {
            try {
                if (reservas == null) {
                    // Cargar una lista vacía temporalmente hasta que el servicio esté disponible
                    reservas = new List<ReservaDTO>();
                }
                DgReservas.ItemsSource = reservas;
            }
            catch (Exception ex) {
                MessageBox.Show($"Error al cargar las reservas: {ex.Message}", "Error de Servicio", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DgReservas_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            BtnSeleccionar.IsEnabled = DgReservas.SelectedItem != null;
        }

        private void BtnSeleccionar_Click(object sender, RoutedEventArgs e) {
            ReservaDTO reservaSeleccionada = DgReservas.SelectedItem as ReservaDTO;
            if (reservaSeleccionada != null) {
                
                _mainFrame.Content = new ReservaDetalleVista(_mainFrame, reservaSeleccionada, _proxyReserva);
            }
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("Funcionalidad de búsqueda no implementada.");
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) {
            // Regresa al menú principal de reservas
            _mainFrame.Content = new ReservaPrincipal(_mainFrame, _proxyReserva, _proxyProducto);
        }
    }
}