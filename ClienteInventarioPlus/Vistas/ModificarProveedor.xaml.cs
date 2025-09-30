using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using ClienteInventarioPlus.Utilidades;

namespace ClienteInventarioPlus.Vistas
{
    public partial class ModificarProveedor : UserControl
    {
        private Frame _mainFrame;
        private ProveedorDTO _proveedor;
        private IProveedorService proxy;
        private string _modo;
        
        public ModificarProveedor(Frame mainFrame, IProveedorService _proxy, ProveedorDTO proveedorModifcar,
            string modo)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _proveedor = proveedorModifcar;
            proxy = _proxy;
            _modo = modo;
            
            tbCorreo.Text = proveedorModifcar.Correo;
            tbDireccion.Text = proveedorModifcar.Direccion;
            tbNombre.Text = proveedorModifcar.Nombre;
            tbTelefono.Text = proveedorModifcar.Telefono;
            
        }


        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Content = new MenuProveedoresAdministrador(_mainFrame, proxy);
        }

        private void btnModificarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (EntradasValidas())
            {
                _proveedor.Nombre = tbNombre.Text;
                _proveedor.Direccion = tbDireccion.Text;
                _proveedor.Correo = tbCorreo.Text;
                _proveedor.Telefono = tbTelefono.Text;

                if (proxy.ActualizarProveedor(_proveedor))
                {
                    MessageBox.Show("Usuario modificado correctamente.");
                    _mainFrame.Content = new ConsultarProveedor(_mainFrame,proxy,_modo);
                }
            }
        }
        
        private bool EntradasValidas()
        {
            bool valido = true;
            
            string errorCorreo = ValidacionesEntrada.ValidarCorreo(tbCorreo);
            string errorNombre = ValidacionesEntrada.ValidarNombre(tbNombre);
            string errorTelefono = ValidacionesEntrada.ValidarTelefono(tbTelefono);
            string errorDireccion = ValidacionesEntrada.ValidarDireccion(tbDireccion);
            
            tblockErrorCorreo.Text = errorCorreo ?? "";
            tblockErrorNombre.Text = errorNombre ?? "";
            tblockErrorUserName.Text = errorTelefono ?? "";
            tblockErrorDireccion.Text = errorDireccion ?? "";

            if ( errorCorreo != null || errorNombre != null || errorTelefono != null || errorDireccion != null)
                valido = false;

            return valido;
        }
        
    }
}