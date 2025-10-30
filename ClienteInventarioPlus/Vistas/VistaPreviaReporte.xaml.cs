using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32; // Necesario para el SaveFileDialog
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;
using PdfiumViewer;

namespace ClienteInventarioPlus.Vistas {
    public partial class ReportePreviaVista : UserControl {
        private readonly string _rutaPdfTemporal;
        private Frame _mainFrame;
        private string nombreDoc;

        public ReportePreviaVista(Frame mainFrame, string rutaPdf, string nombreArchivo) {
            InitializeComponent();
            _mainFrame = mainFrame;
            _rutaPdfTemporal = rutaPdf;
            nombreDoc = nombreArchivo;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            // Muestra el PDF en el control WebBrowser
            if (!string.IsNullOrEmpty(_rutaPdfTemporal) && File.Exists(_rutaPdfTemporal)) {
                PdfViewer.Navigate(new Uri(_rutaPdfTemporal));
            }
            else {
                MessageBox.Show("No se encontró el archivo del reporte para previsualizar.", "Error de Archivo", MessageBoxButton.OK, MessageBoxImage.Error);
                // Si hay un error, regresa a la pantalla anterior
                if (_mainFrame.NavigationService.CanGoBack) {
                    _mainFrame.NavigationService.GoBack();
                }
            }
        }
        
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                PdfViewer.Dispose(); 
            }
            catch { }
        }


        private void BtnDescargar_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog {
                Filter = "Documento PDF (*.pdf)|*.pdf",
                FileName = $"{nombreDoc}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
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
            // Regresa a la pantalla anterior (el menú de reportes)
            if (_mainFrame.NavigationService.CanGoBack) {
                PdfViewer.Dispose();
                _mainFrame.NavigationService.GoBack();
            }
        }
    }
}
