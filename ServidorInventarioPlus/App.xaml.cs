using System;
using System.ServiceModel;
using System.Windows;
using ServidorInventarioPlus.Servicios;

namespace ServidorInventarioPlus
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private ServiceHost _host;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                // 🚀 Aquí levantas tu servicio
                _host = new ServiceHost(typeof(Servicios.UsuarioServicio));
                _host.Open();
                Console.WriteLine("UsuarioServicio levantado y escuchando...");

                // Puedes mostrar una ventana de control si quieres
                var main = new MainWindow();
                main.Show();

                // Ahora sí la app se cierra solo al cerrar la MainWindow
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al levantar servicio: {ex.Message}");
                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // ✅ Cierra el servicio cuando se apaga la app
            _host?.Close();
            base.OnExit(e);
        }
    }
}