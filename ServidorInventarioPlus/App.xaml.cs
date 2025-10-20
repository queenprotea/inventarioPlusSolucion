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
                Console.WriteLine($"Error al levantar servicio este: {ex.Message}");
                Shutdown();
            }
            
            
            try
            {
                // Crear ServiceHost solo con el tipo del servicio
                _host = new ServiceHost(typeof(ProveedorServicio));

                // Abrir el servicio, tomará la configuración del App.config
                _host.Open();

                Console.WriteLine("Servicio pveedorService levantado en:");
                foreach (var endpoint in _host.Description.Endpoints)
                    Console.WriteLine(endpoint.Address);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show($"Error al levantar el servicio: {ex.Message}");
            }
            
            try
            {
                // Crear ServiceHost solo con el tipo del servicio
                _host = new ServiceHost(typeof(ProductoServicio));

                // Abrir el servicio, tomará la configuración del App.config
                _host.Open();

                Console.WriteLine("Servicio ProductoService levantado en:");
                foreach (var endpoint in _host.Description.Endpoints)
                    Console.WriteLine(endpoint.Address);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show($"Error al levantar el servicio: {ex.Message}");
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