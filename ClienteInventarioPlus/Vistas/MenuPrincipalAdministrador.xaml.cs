using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BibliotecaClasesNetframework.ModelosODT;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace ClienteInventarioPlus.Vistas
{
    public partial class MenuPrincipalAdministrador : UserControl
    {
        private MainWindow _mainWindow;
        //private IAhorcadoService proxy;
        private UsuarioDTO usuarioSesion;

        /*public MenuPrincipalAdministrador(MainWindow mainWindow, UsuarioDTO usuario, IAhorcadoService proxy)
        {
            try
            {
                InitializeComponent();
                _mainWindow = mainWindow;
                
                usuarioSesion = usuario;
                this.proxy = proxy;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con el servicio: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }*/
        
        public MenuPrincipalAdministrador(MainWindow mainWindow, UsuarioDTO usuarioActual)
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

        //menu lateral
        private void BtnProductos_Click(object sender, RoutedEventArgs e)
        {
            CambiarSeleccion(BtnProductos);
            //_mainWindow.CambiarVista(new ProductoPrincipal(_mainWindow, jugadorSesion, proxy));
        }

        private void BtnMovimientos_Click(object sender, RoutedEventArgs e)
        {
            CambiarSeleccion(BtnMovimientos);
        }

        private void BtnReservas_Click(object sender, RoutedEventArgs e)
        {
            CambiarSeleccion(BtnReservas);
            //_mainWindow.CambiarVista(new ReservaPrincipal(_mainWindow, jugadorSesion, proxy));
        }

        private void BtnReportes_Click(object sender, RoutedEventArgs e)
        {
            CambiarSeleccion(BtnReportes);
            //_mainWindow.CambiarVista(new ConsultarHistorialPartidasUserControl(_mainWindow, jugadorSesion, proxy));
        }

        private void BtnProveedores_Click(object sender, RoutedEventArgs e)
        {
            CambiarSeleccion(BtnProveedores);
            //_mainWindow.CambiarVista(new MarcadoresUserControl(_mainWindow, jugadorSesion, proxy));
        }
        
        private void BtnUsuarios_Click(object sender, RoutedEventArgs e)
        {
            CambiarSeleccion(BtnUsuarios);
            //_mainWindow.CambiarVista(new MarcadoresUserControl(_mainWindow, jugadorSesion, proxy));
        }
        
        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.CambiarVista(new IniciarSesion(_mainWindow));
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
            
            BtnReportes.Background = Brushes.Black;
            BtnReportes.Foreground = Brushes.White;
            ImgReportes.Source = new BitmapImage(new Uri("/Imagenes/ReporteBlancoLogo.jpg", UriKind.Relative));
            
            BtnProveedores.Background = Brushes.Black;
            BtnProveedores.Foreground = Brushes.White;
            ImgProveedores.Source = new BitmapImage(new Uri("/Imagenes/ProveedorBlancoLogo.jpg", UriKind.Relative));
            
            BtnUsuarios.Background = Brushes.Black;
            BtnUsuarios.Foreground = Brushes.White;
            ImgUsuarios.Source = new BitmapImage(new Uri("/Imagenes/UsuarioBlancoLogo.jpg", UriKind.Relative));

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
                
                case nameof(BtnReportes):
                    ImgReportes.Source = new BitmapImage(new Uri("/Imagenes/ReporteNegroLogo.jpg", UriKind.Relative));
                    break;
                
                case nameof(BtnProveedores):
                    ImgProveedores.Source = new BitmapImage(new Uri("/Imagenes/ProveedorNegroLogo.jpg", UriKind.Relative));
                    break;
                
                case nameof(BtnUsuarios):
                    ImgUsuarios.Source = new BitmapImage(new Uri("/Imagenes/UsuarioNegroLogo.jpg", UriKind.Relative));
                    break;
            }
        }
    }
}