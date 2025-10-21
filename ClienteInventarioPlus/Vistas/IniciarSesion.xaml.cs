using System;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
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
        private IUsuarioService proxy;



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
            InitializeComponent();
            _mainWindow = mainWindow;

            try
            {
                // Crear el proxy usando App.config
                var factory = new ChannelFactory<IUsuarioService>("UsuarioServiceEndpoint");
                proxy = factory.CreateChannel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con el servicio: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Entro aqui");
            if (EntradasValidas())
            {
                Console.WriteLine("Entradas");
                string nombreUsuario = tbUserName.Text.Trim();
                string contrasena = mostrandoPassword ? tbPasswordVisible.Text : pbPassword.Password;

                // Encriptar la contraseña antes de enviarla
                //string passEncriptada = EncriptarContraseña(pass);

                try
                {
                    Console.WriteLine(nombreUsuario);
                    // Llamada al servicio WCF para verificar usuario
                    usuarioActual = proxy.IniciarSesion(nombreUsuario, contrasena);
                  
                    Console.WriteLine(usuarioActual);
                    if (usuarioActual != null)
                    {
                        MessageBox.Show($"¡Bienvenido {usuarioActual.Nombre}!");
                        
                        if (usuarioActual.Rol == "Administrador")
                        {
                            _mainWindow.CambiarVista(new MenuPrincipalAdministrador(_mainWindow, usuarioActual));
                        }
                        else if (usuarioActual.Rol == "Empleado")
                        {
                            _mainWindow.CambiarVista(new MenuPrincipalEmpleado(_mainWindow, usuarioActual));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Usuario o contraseña incorrectos", "Error", MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }



      

        private bool EntradasValidas()
        {
            bool valido = true;
            string usname = ValidacionesEntrada.ValidarNombreUsuario(tbUserName);
            string pass = ValidacionesEntrada.ValidarPassword(pbPassword);
            
            tblockErrorUserName.Text = usname ?? "";
            tblockErrorPassword.Text = pass ?? "";

            if (usname != null || pass != null)
                valido = false;

            return valido;
        }



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

