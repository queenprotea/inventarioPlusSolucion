using System;
using System.ServiceModel;
using System.Windows;
using ServidorInventarioPlus.Servicios;

namespace ServidorInventarioPlus
{
    public partial class MainWindow
    {
        private ServiceHost _host;

        public MainWindow()
        {
            InitializeComponent();
            IniciarServicioWCF();
        }

        private void IniciarServicioWCF()
        {
            try
            {
                // Crear ServiceHost solo con el tipo del servicio
                _host = new ServiceHost(typeof(UsuarioServicio));

                // Abrir el servicio, tomará la configuración del App.config
                _host.Open();

                Console.WriteLine("Servicio UsuarioService levantado en:");
                foreach (var endpoint in _host.Description.Endpoints)
                    Console.WriteLine(endpoint.Address);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show($"Error al levantar el servicio: {ex.Message}");
            }
        }

        // Cierra el servicio al cerrar la app
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (_host != null)
            {
                if (_host.State == CommunicationState.Faulted)
                    _host.Abort();
                else
                    _host.Close();
            }
            base.OnClosing(e);
        }
    }
}