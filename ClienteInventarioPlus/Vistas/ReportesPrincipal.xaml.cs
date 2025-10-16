using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace ClienteInventarioPlus.Vistas {
    public partial class ReportesPrincipal : UserControl {
        private readonly IReporteService _proxyReporte;
        private Frame _mainFrame;

        public ReportesPrincipal(Frame mainFrame, IReporteService proxyReporte) {
            InitializeComponent();
            _mainFrame = mainFrame;
            _proxyReporte = proxyReporte;
        }

        private void BtnReporteInventario_Click(object sender, RoutedEventArgs e) {
            // Pendiente de integración con el servicio real de reportes.
            MessageBox.Show("La generación del reporte de inventario estará disponible cuando el servicio esté integrado.");
        }

        private void BtnReporteMovimientos_Click(object sender, RoutedEventArgs e) {
            
            _mainFrame.Content = new SeleccionarPeriodoVista(_mainFrame, _proxyReporte, "Movimientos");
        }
    }
}