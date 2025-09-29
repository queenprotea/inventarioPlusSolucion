using System;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;

namespace ClienteInventarioPlus.Vistas
{
    
    public partial class AgregarProveedor : UserControl
    
    {
        private Frame _mainFrame;
        public AgregarProveedor(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }
        

        private void BtnGuardar_OnClickbtnGuardar(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Entró al registro de proveedor");

            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(this.nombre.Text) ||
                string.IsNullOrWhiteSpace(this.correo.Text) ||
                string.IsNullOrWhiteSpace(this.direccion.Text)||
                String.IsNullOrWhiteSpace(this.telefono.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Capturar los datos del proveedor
            string nombre = this.nombre.Text.Trim();
            string direccion = this.direccion.Text.Trim();
            string telefono = this.telefono.Text.Trim();
            string correo = this.correo.Text.Trim();

            // Crear objeto DTO
            var proveedor = new ProveedorDTO
            {
                Nombre = nombre,
                Direccion = direccion,
                Telefono = telefono,
                Correo = correo,
            };

            try
            {
                Console.WriteLine($"Registrando proveedor: {nombre}");
        
                

              
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al comunicarse con el servicio: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void clickbtnCancelar(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}