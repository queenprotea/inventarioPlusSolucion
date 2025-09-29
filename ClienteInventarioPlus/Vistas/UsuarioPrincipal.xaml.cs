using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;

namespace ClienteInventarioPlus.Vistas
{
    public partial class UsuarioPrincipal : UserControl
    {
        private Frame _mainFrame;
        private UsuarioDTO _usuarioSesion;
        private IUsuarioService proxy;
        
        public UsuarioPrincipal(Frame mainFrame, UsuarioDTO usuarioActual, IUsuarioService _proxy)
        {
            _mainFrame = mainFrame;
            _usuarioSesion = usuarioActual;
            proxy = _proxy;
            InitializeComponent();
        }

        private void btnRegistrarUsuario_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Content = new RegistrarUsuario(_mainFrame, _usuarioSesion, proxy);
        }

        private void btnConsultarUsuarios_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Content = new ConsultarUsuarios(_mainFrame, _usuarioSesion, proxy);
        }
    }
}