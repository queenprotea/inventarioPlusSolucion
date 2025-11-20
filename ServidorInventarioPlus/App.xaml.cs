using System;
using System.ServiceModel;
using System.Windows;
using ServidorInventarioPlus.Servicios;

namespace ServidorInventarioPlus
{
    public partial class App : Application
    {
        private ServiceHost[] _hosts;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _hosts = new[]
            {
                new ServiceHost(typeof(UsuarioServicio)),
                new ServiceHost(typeof(ProveedorServicio)),
                new ServiceHost(typeof(ProductoServicio)),
                new ServiceHost(typeof(MovimientoServicio)),
                new ServiceHost(typeof(ReservaServicio))
            };

            try
            {
                foreach (var host in _hosts)
                {
                    host.Open();
                }

                MessageBox.Show("SERVIDOR INVENTARIO PLUS LEVANTADO CORRECTAMENTE");

                // Mostrar ventana principal
                MainWindow window = new MainWindow();
                window.Show();

                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR al iniciar servicios:\n\n" + ex.ToString());
                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_hosts != null)
            {
                foreach (var host in _hosts)
                {
                    try
                    {
                        host?.Close();
                    }
                    catch { /* Ignore */ }
                }
            }

            base.OnExit(e);
        }
    }
}