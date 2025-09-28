
using System.Windows;
using System.Windows.Controls;


namespace ClienteInventarioPlus.Vistas
{
    public partial class InicioSesionGUI : UserControl
    {
        public InicioSesionGUI()
        {
            InitializeComponent();
        }

        // Botón para mostrar/ocultar contraseña
        private void btnVerPassword_Click(object sender, RoutedEventArgs e)
        {
            if (pbPassword.Visibility == Visibility.Visible)
            {
                // Mostrar la contraseña en el TextBox
                tbPasswordVisible.Text = pbPassword.Password;
                pbPassword.Visibility = Visibility.Collapsed;
                tbPasswordVisible.Visibility = Visibility.Visible;

                // Cambiar el icono del botón (opcional)
                btnVerPassword.Content = "🙈";
            }
            else
            {
                // Ocultar la contraseña nuevamente
                pbPassword.Password = tbPasswordVisible.Text;
                pbPassword.Visibility = Visibility.Visible;
                tbPasswordVisible.Visibility = Visibility.Collapsed;

                // Cambiar el icono del botón (opcional)
                btnVerPassword.Content = "👁";
            }
        }

        // Botón para iniciar sesión
        private void btnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            string usuario = tbUserName.Text;
            string contrasena = string.Empty;

            // Dependiendo de qué control esté visible, tomo la contraseña
            if (pbPassword.Visibility == Visibility.Visible)
                contrasena = pbPassword.Password;
            else
                contrasena = tbPasswordVisible.Text;

            if (string.IsNullOrWhiteSpace(usuario))
            {
                tblockErrorUserName.Text = "El nombre de usuario es obligatorio.";
                return;
            }
            else
            {
                tblockErrorUserName.Text = "";
            }

            if (string.IsNullOrWhiteSpace(contrasena))
            {
                tblockErrorPassword.Text = "La contraseña es obligatoria.";
                return;
            }
            else
            {
                tblockErrorPassword.Text = "";
            }

            // Aquí puedes agregar tu lógica real de autenticación
            MessageBox.Show($"Usuario: {usuario}\nContraseña: {contrasena}", "Inicio de Sesión");
        }
    }
}
