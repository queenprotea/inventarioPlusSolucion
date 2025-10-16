using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32; // Necesario para el SaveFileDialog
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace ClienteInventarioPlus.Vistas {
    public partial class ReportePreviaMovimientosVista : UserControl {
        private readonly string _rutaPdfTemporal;
        private Frame _mainFrame;

        public ReportePreviaMovimientosVista(Frame mainFrame, string rutaPdf) {
            InitializeComponent();
            _mainFrame = mainFrame;
            _rutaPdfTemporal = rutaPdf;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            // Muestra el PDF en el control WebBrowser
            if (!string.IsNullOrEmpty(_rutaPdfTemporal) && File.Exists(_rutaPdfTemporal)) {
                PdfViewer.Navigate(new Uri(_rutaPdfTemporal));
            }
            else {
                MessageBox.Show("No se encontró el archivo del reporte para previsualizar.", "Error de Archivo", MessageBoxButton.OK, MessageBoxImage.Error);
                if (_mainFrame.NavigationService.CanGoBack) {
                    _mainFrame.NavigationService.GoBack();
                }
            }
        }

        private void BtnDescargar_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog {
                Filter = "Documento PDF (*.pdf)|*.pdf",
                FileName = $"Reporte_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                Title = "Guardar Reporte Como"
            };

            if (saveFileDialog.ShowDialog() == true) {
                try {
                    // Copia el archivo temporal a la ubicación elegida por el usuario
                    File.Copy(_rutaPdfTemporal, saveFileDialog.FileName, true);
                    MessageBox.Show($"Reporte guardado exitosamente en:\n{saveFileDialog.FileName}", "Descarga Completa", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (_mainFrame.NavigationService.CanGoBack) {
                        _mainFrame.NavigationService.GoBack();
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show($"Error al guardar el archivo: {ex.Message}", "Error de Guardado", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) {
            // Regresa a la pantalla anterior (el menú de reportes o el selector de fecha)
            if (_mainFrame.NavigationService.CanGoBack) {
                _mainFrame.NavigationService.GoBack();
            }
        }
    }
}