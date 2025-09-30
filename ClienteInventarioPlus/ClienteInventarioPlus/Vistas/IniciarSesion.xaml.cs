using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.ModelosODT;
using ClienteInventarioPlus.Utilidades;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace ClienteInventarioPlus.Vistas
{
    public partial class IniciarSesion : UserControl
    {
        private MainWindow _mainWindow;
        private bool mostrandoPassword = false;
        //IAhorcadoService proxy;
        UsuarioDTO usuarioActual;



        /*public IniciarSesion(MainWindow mainWindow, IAhorcadoService proxy)
        {
            try
            {
                InitializeComponent();
                _mainWindow = mainWindow;
                this.proxy = proxy;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con el servicio: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }*/
        
        public IniciarSesion(MainWindow mainWindow)
        {
            try
            {
                InitializeComponent();
                _mainWindow = mainWindow;
                //this.proxy = proxy;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con el servicio: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*private void btnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            if (EntradasValidas())
            {
                var correo = tbUserName.Text;
                string pass;

                if (mostrandoPassword)
                    pass = tbPasswordVisible.Text;
                else
                    pass = pbPassword.Password;

                pass = EncriptarContraseña(pass.Trim());

                try
                {
                    //usuarioActual = proxy.IniciarSesion(correo, pass);

                    if (usuarioActual != null)
                    {
                        string mensajeBienvenida = Application.Current.TryFindResource("Msg_Titulo_Bienvenida") as string ?? "¡Bienvenido!";

                        MessageBox.Show(mensajeBienvenida);
                        MostrarMenuPrincipal(usuarioActual);
                    }
                    else
                    {
                        string mensajeErrorSesion = Application.Current.TryFindResource("Msg_Error_InicioSesion") as string;

                        MessageBox.Show(mensajeErrorSesion);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }*/

        private void btnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.CambiarVista(new MenuPrincipalAdministrador(_mainWindow));
        }

        private bool EntradasValidas()
        {
            bool valido = true;
            string correo = ValidacionesEntrada.ValidarCorreo(tbUserName);
            string pass = ValidacionesEntrada.ValidarPassword(pbPassword);

            string ErrorCorreo = !string.IsNullOrEmpty(correo)
                ? Application.Current.TryFindResource(correo) as string : null;

            string ErrorPassword = !string.IsNullOrEmpty(pass)
                ? Application.Current.TryFindResource(pass) as string : null;

            tblockErrorUserName.Text = ErrorCorreo ?? "";
            tblockErrorPassword.Text = ErrorPassword ?? "";

            if (correo != null || pass != null)
                valido = false;

            return valido;
        }

        // ¡IMPORTANTE! Aquí pasas el proxy a la siguiente ventana
        /*private void MostrarMenuPrincipal(UsuarioDTO usuario)
        {
            _mainWindow.CambiarVista(new MenuPrincipalAdministrador(_mainWindow, usuario));//, proxy));
        }*/
        

        private void btnVerPassword_Click(object sender, RoutedEventArgs e)
        {
            mostrandoPassword = !mostrandoPassword;
            if (mostrandoPassword)
            {
                tbPasswordVisible.Text = pbPassword.Password;
                tbPasswordVisible.Visibility = Visibility.Visible;
                pbPassword.Visibility = Visibility.Collapsed;
                btnVerPassword.Content = "🙈";
            }
            else
            {
                pbPassword.Password = tbPasswordVisible.Text;
                pbPassword.Visibility = Visibility.Visible;
                tbPasswordVisible.Visibility = Visibility.Collapsed;
                btnVerPassword.Content = "👁";
            }
        }

        private void PbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!mostrandoPassword)
                tbPasswordVisible.Text = pbPassword.Password;
        }

        private void TbPasswordVisible_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (mostrandoPassword)
                pbPassword.Password = tbPasswordVisible.Text;
        }
        
        public static string EncriptarContraseña(string contraseña)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(contraseña));
                StringBuilder builder = new StringBuilder();
                foreach (var byteValue in bytes)
                {
                    builder.Append(byteValue.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}