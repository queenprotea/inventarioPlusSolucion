using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;

namespace ClienteInventarioPlus.Vistas
{
    public partial class MenuPrincipalEmpleado : UserControl
    {
        private MainWindow _mainWindow;
        private UsuarioDTO usuarioSesion;
        private IMovimientoService proxyMovimiento;
        private IReservaService proxyReserva;
        private IProductoService proxyProducto;
        private IProveedorService proxyProveedor;
        
        public MenuPrincipalEmpleado(MainWindow mainWindow, UsuarioDTO usuarioActual)
        {
            try
            {
                InitializeComponent();
                _mainWindow = mainWindow;
                usuarioSesion = usuarioActual;
                
                // Crear el proxy usando App.config
                
                var factoryProdcuto = new ChannelFactory<IProductoService>("ProductoServiceEndpoint");
                proxyProducto = factoryProdcuto.CreateChannel();
                
                var factoryProveeedor = new ChannelFactory<IProveedorService>("ProveedorServiceEndpoint");
                proxyProveedor = factoryProveeedor.CreateChannel();
                
                // Cargar vista de productos por defecto
                MainFrame.Content = new ProductoPrincipal(MainFrame, proxyProducto, proxyProveedor);
                CambiarSeleccion(BtnProductos);
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
            var factory = new ChannelFactory<IReservaService>("ReservaServiceEndpoint");
            proxyReserva = factory.CreateChannel();
            MainFrame.Content = new ReservaPrincipal(MainFrame, proxyReserva, proxyProducto);
        }

        private void BtnMovimientos_Click(object sender, RoutedEventArgs e)
        {
            var factory = new ChannelFactory<IMovimientoService>("MovimientoServiceEndpoint");
            proxyMovimiento = factory.CreateChannel();
            CambiarSeleccion(BtnMovimientos); 
            MainFrame.Content = new MovimientoPrincipal(MainFrame ,usuarioSesion, proxyMovimiento);
        }

        private void BtnProductos_Click(object sender, RoutedEventArgs e)
        {
            CambiarSeleccion(BtnProductos);
            MainFrame.Content = new ProductoPrincipal(MainFrame, proxyProducto, proxyProveedor);
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
            }
        }
    }
}