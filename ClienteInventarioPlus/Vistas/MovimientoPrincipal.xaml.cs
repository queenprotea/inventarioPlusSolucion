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
        public MovimientoPrincipal(Frame mainFrame, UsuarioDTO usuarioActual, IMovimientoService _proxy)
        {
            _mainFrame = mainFrame;
            _usuarioSesion = usuarioActual;
            proxy = _proxy;
            InitializeComponent();
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