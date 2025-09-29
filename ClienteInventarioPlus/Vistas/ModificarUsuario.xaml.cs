using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using ClienteInventarioPlus.Utilidades;

namespace ClienteInventarioPlus.Vistas
{
    public partial class ModificarUsuario : UserControl
    {
        
        private Frame _mainFrame;
        private UsuarioDTO _usuarioSesion;
        private IUsuarioService proxy;
        private UsuarioDTO _usuario;

        
        public ModificarUsuario(Frame mainFrame, UsuarioDTO usuarioActual, IUsuarioService _proxy, UsuarioDTO usuarioModifica)
        {
            _mainFrame = mainFrame;
            _usuarioSesion = usuarioActual;
            proxy = _proxy;
            _usuario = usuarioModifica;
            InitializeComponent();
            
            // Llenar los campos con los datos actuales
            tbNombre.Text = usuarioModifica.Nombre;
            tbUsername.Text = usuarioModifica.NombreUsuario;
            tbPassword.Text = usuarioModifica.Contrasena;

            if (usuarioModifica.Rol == "Administrador")
                rbAdministrador.IsChecked = true;
            else
                rbEmpleado.IsChecked = true;
        }

        private void btnModificarUsuario_Click(object sender, RoutedEventArgs e)
        {

            if (EntradasValidas())
            {
                // Actualizamos objeto
                _usuario.Nombre = tbNombre.Text;
                _usuario.NombreUsuario = tbUsername.Text;
                _usuario.Contrasena = tbPassword.Text;
                _usuario.Rol = rbAdministrador.IsChecked == true ? "Administrador" : "Empleado";

                if (proxy.ModificarUsuario(_usuario))
                {
                    MessageBox.Show("Usuario modificado correctamente.");
                    _mainFrame.Content = new ConsultarUsuarios(_mainFrame, _usuario, proxy);
                }
                else
                {
                    MessageBox.Show("Error al modificar usuario.");
                }

            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Content = new ConsultarUsuarios(_mainFrame, _usuarioSesion, proxy);
        }
        
        private bool EntradasValidas()
        {
            bool valido = true;
            
            string errorPass = ValidacionesEntrada.ValidarPasswordTextBox(tbPassword);
            string errorNombre = ValidacionesEntrada.ValidarNombre(tbNombre);
            string errorUserName = ValidacionesEntrada.ValidarNombreUsuario(tbUsername);
            string errorRol = ValidacionesEntrada.ValidarRadioButtons(rbAdministrador, rbEmpleado);
            
            tblockErrorPassword.Text = errorPass ?? "";
            tblockErrorNombre.Text = errorNombre ?? "";
            tblockErrorUserName.Text = errorUserName ?? "";
            tblockErrorRol.Text = errorRol ?? "";

            if ( errorPass != null || errorNombre != null || errorUserName != null || errorRol != null)
                valido = false;

            return valido;
        }
    }
}