using System;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using ClienteInventarioPlus.Utilidades;

namespace ClienteInventarioPlus.Vistas
{
    
    public partial class AgregarProveedor : UserControl
    
    {
        private Frame _mainFrame;
        private IProveedorService proxy;
        public AgregarProveedor(Frame mainFrame, IProveedorService _proxy)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            proxy = _proxy;
        }
        

        private void BtnGuardar_OnClickbtnGuardar(object sender, RoutedEventArgs e)
        {
            bool registroExitoso = false;

            if (EntradasValidas())
            {
                if (proxy == null)
                {
                    MessageBox.Show("El servicio no está disponible. Inténtelo más tarde.");
                    return;
                }
                
                ProveedorDTO proveedorRegistro = new ProveedorDTO();

                proveedorRegistro.Nombre = nombre.Text.Trim();
                proveedorRegistro.Correo = correo.Text.Trim();
                proveedorRegistro.Telefono = telefono.Text.Trim();
                proveedorRegistro.Direccion = direccion.Text.Trim();
                
                registroExitoso = proxy.AgregarProveedor(proveedorRegistro);

                if (registroExitoso)
                {
                    MessageBox.Show("Registro Exitoso");
                    _mainFrame.Content = new MenuProveedoresAdministrador(_mainFrame, proxy);
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
            
            string errorCorreo = ValidacionesEntrada.ValidarCorreo(correo);
            string errorNombre = ValidacionesEntrada.ValidarNombre(nombre);
            string errorTelefono = ValidacionesEntrada.ValidarTelefono(telefono);
            string errorDireccion = ValidacionesEntrada.ValidarDireccion(direccion);
            
            tblockErrorCorreo.Text = errorCorreo ?? "";
            tblockErrorNombre.Text = errorNombre ?? "";
            tblockErrorTelefono.Text = errorTelefono ?? "";
            tblockErrorDireccion.Text = errorDireccion ?? "";

            if ( errorCorreo != null || errorNombre != null || errorTelefono != null || errorDireccion != null)
                valido = false;

            return valido;
        }

        private void clickbtnCancelar(object sender, RoutedEventArgs e)
        {
            _mainFrame.Content = new MenuProveedoresAdministrador(_mainFrame, proxy);
        }
    }
}