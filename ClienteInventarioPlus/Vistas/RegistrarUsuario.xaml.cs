using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using ClienteInventarioPlus.Utilidades;

namespace ClienteInventarioPlus.Vistas
{
    public partial class RegistrarUsuario : UserControl
    {
        private Frame _mainFrame;
        private UsuarioDTO _usuarioSesion;
        private IUsuarioService proxy;
        public RegistrarUsuario(Frame mainFrame, UsuarioDTO usuarioActual, IUsuarioService _proxy)
        {
            _mainFrame = mainFrame;
            _usuarioSesion = usuarioActual;
            proxy = _proxy;
            InitializeComponent();
            
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Content = new UsuarioPrincipal(_mainFrame, _usuarioSesion, proxy);
        }

        private void btnRegistrarUsuario_Click(object sender, RoutedEventArgs e)
        {
            bool registroExitoso = false;

            if (EntradasValidas())
            {
                if (proxy == null)
                {
                    MessageBox.Show("El servicio no está disponible. Inténtelo más tarde.");
                    return;
                }
                
                string opcionSeleccionada = null;

                if (rbAdministrador.IsChecked == true)
                    opcionSeleccionada = rbAdministrador.Content.ToString();
                else if (rbEmpleado.IsChecked == true)
                    opcionSeleccionada = rbEmpleado.Content.ToString();

                UsuarioDTO usuarioRegistro = new UsuarioDTO();
                
                usuarioRegistro.Nombre = tbNombre.Text.Trim();
                usuarioRegistro.NombreUsuario = tbUsername.Text.Trim();
                usuarioRegistro.Contrasena = tbPassword.Text.Trim();
                usuarioRegistro.Rol = opcionSeleccionada; 


                registroExitoso = proxy.RegistrarUsuario(usuarioRegistro);

                if (registroExitoso)
                {
                    MessageBox.Show("Registro Exitoso");
                    _mainFrame.Content = new UsuarioPrincipal(_mainFrame, _usuarioSesion, proxy);
                }
                else
                {
                    MessageBox.Show("Error al registrar");
                }
            }
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