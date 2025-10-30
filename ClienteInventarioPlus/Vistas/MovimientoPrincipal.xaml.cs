using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;

namespace ClienteInventarioPlus.Vistas
{
    public partial class MovimientoPrincipal : UserControl
    {
        private Frame _mainFrame;
        private UsuarioDTO _usuarioSesion;
        private IMovimientoService proxy;
        private IProductoService proxyProducto;
        public MovimientoPrincipal(Frame mainFrame, UsuarioDTO usuarioActual, IMovimientoService _proxy)
        {
            _mainFrame = mainFrame;
            _usuarioSesion = usuarioActual;
            proxy = _proxy;
            InitializeComponent();
            
            var factoryProdcuto = new ChannelFactory<IProductoService>("ProductoServiceEndpoint");
            proxyProducto = factoryProdcuto.CreateChannel();
            
            MostrarProductosStockBajo();
        }
        
        private void MostrarProductosStockBajo()
        {
            try
            {
                // Llamamos al método que devuelve los productos con stock bajo
                var lista = proxyProducto.ObtenerProductosConStockBajo();

                if (lista.Count > 0)
                {
                    // Construimos el mensaje con los nombres y stock de los productos
                    string mensaje = "Productos con stock bajo:\n\n";
                    foreach (var prod in lista)
                    {
                        mensaje += $"({prod.Codigo}) {prod.Nombre} - Stock: {prod.Stock} (Stock mínimo: {prod.StockMinimo})\n";
                    }

                    MessageBox.Show(
                        mensaje,
                        "Productos con stock bajo",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}");
            }
        }


        private void btnRegistrarEntrada_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Content = new RegistrarEntrada(_mainFrame, _usuarioSesion, proxy);
        }

        private void btnConsultarHistorialMovimientos_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Content = new HistorialMovimientos(_mainFrame, _usuarioSesion, proxy);
        }

        private void btnRegistrarSalida_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Content = new RegistrarSalida(_mainFrame, _usuarioSesion, proxy);
        }
    }
}