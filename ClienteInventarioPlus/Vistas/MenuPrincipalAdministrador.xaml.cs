using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace ClienteInventarioPlus.Vistas
{
    public partial class MenuPrincipalAdministrador : UserControl
    {
        private MainWindow _mainWindow;
        //private IAhorcadoService proxy;
        private UsuarioDTO usuarioSesion;
        private IUsuarioService proxy;
        private IProveedorService proxyProveedor;
        private IProductoService proxyProducto;
        private IReservaService proxyReserva;
        private IMovimientoService proxyMovimiento;

        
        public MenuPrincipalAdministrador(MainWindow mainWindow, UsuarioDTO usuario)
        {
            try
            {
                InitializeComponent();
                _mainWindow = mainWindow;
                usuarioSesion = usuario;
                
                // Crear el proxy usando App.config
                var factory = new ChannelFactory<IUsuarioService>("UsuarioServiceEndpoint");
                proxy = factory.CreateChannel();
                
                var factoryProdcuto = new ChannelFactory<IProductoService>("ProductoServiceEndpoint");
                proxyProducto = factoryProdcuto.CreateChannel();
                
                var factoryProveeedor = new ChannelFactory<IProveedorService>("ProveedorServiceEndpoint");
                proxyProveedor = factoryProveeedor.CreateChannel();
                
                var factoryMovimiento = new ChannelFactory<IMovimientoService>("MovimientoServiceEndpoint");
                proxyMovimiento = factoryMovimiento.CreateChannel();
                
                // Cargar vista de productos por defecto
                MainFrame.Content = new ProductoPrincipal(MainFrame, proxyProducto, proxyProveedor);
                CambiarSeleccion(BtnProductos);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con el servicio: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            MainFrame.Content = new ConsultarProductoAdmin(proxyProducto, proxyProveedor, MainFrame);
        }

        //menu lateral
        private void BtnProductos_Click(object sender, RoutedEventArgs e)
        {
            CambiarSeleccion(BtnProductos);
            MainFrame.Content = new ProductoPrincipal(MainFrame, proxyProducto, proxyProveedor);
        }

        private void BtnMovimientos_Click(object sender, RoutedEventArgs e)
        {
            CambiarSeleccion(BtnMovimientos); 
            MainFrame.Content = new MovimientoPrincipal(MainFrame ,usuarioSesion, proxyMovimiento);
        }

        private void BtnReservas_Click(object sender, RoutedEventArgs e)
        {
            CambiarSeleccion(BtnReservas);
            var factory = new ChannelFactory<IReservaService>("ReservaServiceEndpoint");
            proxyReserva = factory.CreateChannel();
            MainFrame.Content = new ReservaPrincipal(MainFrame, proxyReserva, proxyProducto);
        }

        private void BtnReportes_Click(object sender, RoutedEventArgs e)
        {
            CambiarSeleccion(BtnReportes);
            MainFrame.Content = new ReportesPrincipal(MainFrame, proxyProducto, proxyMovimiento, usuarioSesion);
        }

        private void BtnProveedores_Click(object sender, RoutedEventArgs e)
        {
            var factory = new ChannelFactory<IProveedorService>("ProveedorServiceEndpoint");
            proxyProveedor = factory.CreateChannel();
            CambiarSeleccion(BtnProveedores);
            MainFrame.Content = new MenuProveedoresAdministrador(MainFrame,proxyProveedor);
        }
        
        private void BtnUsuarios_Click(object sender, RoutedEventArgs e)
        {
            CambiarSeleccion(BtnUsuarios);
            MainFrame.Content = new UsuarioPrincipal(MainFrame ,usuarioSesion, proxy);
        }
        
        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.CambiarVista(new IniciarSesion(_mainWindow));
        }

        

        private void CambiarSeleccion(Button botonActivo)
        {
            // Resetear todos los botones
            BtnProductos.Background = Brushes.Black;
            BtnProductos.Foreground = Brushes.White;
            ImgProductos.Fill = Brushes.White;

            BtnMovimientos.Background = Brushes.Black;
            BtnMovimientos.Foreground = Brushes.White;
            ImgMovimientos.Fill = Brushes.White;
            
            BtnReservas.Background = Brushes.Black;
            BtnReservas.Foreground = Brushes.White;
            ImgReservas.Fill = Brushes.White;
            
            BtnReportes.Background = Brushes.Black;
            BtnReportes.Foreground = Brushes.White;
            ImgReportes.Fill = Brushes.White;
            
            BtnProveedores.Background = Brushes.Black;
            BtnProveedores.Foreground = Brushes.White;
            ImgProveedores.Fill = Brushes.White;
            
            BtnUsuarios.Background = Brushes.Black;
            BtnUsuarios.Foreground = Brushes.White;
            ImgUsuarios.Fill = Brushes.White;

            // Marcar botón activo
            botonActivo.Background = Brushes.White;
            botonActivo.Foreground = Brushes.Black;

            // Cambiar color del icono activo a negro
            switch (botonActivo.Name)
            {
                case nameof(BtnProductos):
                    ImgProductos.Fill = Brushes.Black;
                    break;

                case nameof(BtnMovimientos):
                    ImgMovimientos.Fill = Brushes.Black;
                    break;

                case nameof(BtnReservas):
                    ImgReservas.Fill = Brushes.Black;
                    break;
                
                case nameof(BtnReportes):
                    ImgReportes.Fill = Brushes.Black;
                    break;
                
                case nameof(BtnProveedores):
                    ImgProveedores.Fill = Brushes.Black;
                    break;
                
                case nameof(BtnUsuarios):
                    ImgUsuarios.Fill = Brushes.Black;
                    break;
            }
        }
    }
}