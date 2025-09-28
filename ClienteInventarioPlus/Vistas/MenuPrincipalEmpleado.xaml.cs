using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BibliotecaClasesNetframework.ModelosODT;

namespace ClienteInventarioPlus.Vistas
{
    public partial class MenuPrincipalEmpleado : UserControl
    {
        private MainWindow _mainWindow;
        private UsuarioDTO usuarioSesion;
        
        public MenuPrincipalEmpleado(MainWindow mainWindow, UsuarioDTO usuarioActual)
        {
            try
            {
                InitializeComponent();
                _mainWindow = mainWindow;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con el servicio: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.CambiarVista(new IniciarSesion(_mainWindow));
        }

        private void BtnReservas_Click(object sender, RoutedEventArgs e)
        {
            CambiarSeleccion(BtnReservas);
        }

        private void BtnMovimientos_Click(object sender, RoutedEventArgs e)
        {
            CambiarSeleccion(BtnMovimientos);
        }

        private void BtnProductos_Click(object sender, RoutedEventArgs e)
        {
            CambiarSeleccion(BtnProductos);
        }
        
        private void CambiarSeleccion(Button botonActivo)
        {
            // Resetear todos
            BtnProductos.Background = Brushes.Black;
            BtnProductos.Foreground = Brushes.White;
            ImgProductos.Source = new BitmapImage(new Uri("/Imagenes/ProductoBlancoLogo.jpg", UriKind.Relative));

            BtnMovimientos.Background = Brushes.Black;
            BtnMovimientos.Foreground = Brushes.White;
            ImgMovimientos.Source = new BitmapImage(new Uri("/Imagenes/MovimientoBlancoLogo.jpg", UriKind.Relative));
            
            BtnReservas.Background = Brushes.Black;
            BtnReservas.Foreground = Brushes.White;
            ImgReservas.Source = new BitmapImage(new Uri("/Imagenes/ReservasBlancoLogo.jpg", UriKind.Relative));
            
            // Marcar activo
            botonActivo.Background = Brushes.White;
            botonActivo.Foreground = Brushes.Black;

            switch (botonActivo.Name)
            {
                case nameof(BtnProductos):
                    ImgProductos.Source = new BitmapImage(new Uri("/Imagenes/ProductoNegroLogo.jpg", UriKind.Relative));
                    break;

                case nameof(BtnMovimientos):
                    ImgMovimientos.Source = new BitmapImage(new Uri("/Imagenes/MovimientoNegroLogo.jpg", UriKind.Relative));
                    break;

                case nameof(BtnReservas):
                    ImgReservas.Source = new BitmapImage(new Uri("/Imagenes/ReservaNegroLogo.jpg", UriKind.Relative));
                    break;
                
                
            }
        }
    }
}