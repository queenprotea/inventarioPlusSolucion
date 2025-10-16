using System;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace ClienteInventarioPlus.Vistas {
    public partial class SeleccionarPeriodoVista : UserControl {
        private readonly IReporteService _proxyReporte;
        private Frame _mainFrame;
        private readonly string _tipoReporte;

        // El constructor recibe el tipo de reporte para saber cuál generar
        public SeleccionarPeriodoVista(Frame mainFrame, IReporteService proxyReporte, string tipoReporte) {
            InitializeComponent();
            _mainFrame = mainFrame;
            _proxyReporte = proxyReporte;
            _tipoReporte = tipoReporte;
        }

        private void BtnGenerar_Click(object sender, RoutedEventArgs e) {
            // --- 1. Validar Fechas ---
            if (DpFechaInicio.SelectedDate == null || DpFechaFin.SelectedDate == null) {
                MessageBox.Show("Debe seleccionar una fecha de inicio y una fecha final.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime fechaInicio = DpFechaInicio.SelectedDate.Value;
            DateTime fechaFin = DpFechaFin.SelectedDate.Value;

            if (fechaFin < fechaInicio) {
                MessageBox.Show("La fecha final no puede ser anterior a la fecha inicial.", "Fechas Inválidas", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // --- 2. Llamar al Servicio y Navegar a la Vista Previa ---
            try {
                MessageBox.Show("Generando reporte, por favor espere...", "Procesando");

                // En esta versión, solo mostramos un mensaje hasta tener el servicio disponible
                MessageBox.Show($"Se generará el reporte de '{_tipoReporte}' para el período {fechaInicio:d} - {fechaFin:d} cuando el servicio esté integrado.");
            }
            catch (Exception ex) {
                MessageBox.Show($"Ocurrió un error al generar el reporte: {ex.Message}", "Error de Servicio", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) {
            if (_mainFrame.NavigationService.CanGoBack) {
                _mainFrame.NavigationService.GoBack();
            }
        }
    }
}
