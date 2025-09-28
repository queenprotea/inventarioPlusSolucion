using System;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.ModelosODT;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace ClienteInventarioPlus.Vistas
{
    public partial class MenuPrincipalAdministrador : UserControl
    {
        private MainWindow _mainWindow;
        //private IAhorcadoService proxy;
        private UsuarioDTO usuarioSesion;

     
        
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
            //_mainWindow.CambiarVista(new ProductoPrincipal(_mainWindow, jugadorSesion, proxy));
        }

        private void BtnMovimientos_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnReservas_Click(object sender, RoutedEventArgs e)
        {
            //_mainWindow.CambiarVista(new ReservaPrincipal(_mainWindow, jugadorSesion, proxy));
        }

        private void BtnReportes_Click(object sender, RoutedEventArgs e)
        {
            //_mainWindow.CambiarVista(new ConsultarHistorialPartidasUserControl(_mainWindow, jugadorSesion, proxy));
        }

        private void BtnProveedores_Click(object sender, RoutedEventArgs e)
        {
            // Solo actualizamos la sección de contenido
            MainFrame.Content = new MenuProveedoresAdministrador(MainFrame);
        }

        
        private void BtnUsuarios_Click(object sender, RoutedEventArgs e)
        {
            //_mainWindow.CambiarVista(new MarcadoresUserControl(_mainWindow, jugadorSesion, proxy));
        }

        /*private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.CambiarVista(new IniciarSesion(_mainWindow, proxy));
        }*/
        
        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.CambiarVista(new IniciarSesion(_mainWindow));
        }
        
    }
}