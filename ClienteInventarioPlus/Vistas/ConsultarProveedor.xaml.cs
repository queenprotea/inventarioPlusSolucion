using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;

namespace ClienteInventarioPlus.Vistas
{
    public partial class ConsultarProveedor : UserControl
    {
        
        private Frame _mainFrame;
        private IProveedorService proxy;
        public ObservableCollection<ProveedorDTO> Proveedores { get; set; } = new ObservableCollection<ProveedorDTO>();
        public ConsultarProveedor(Frame mainFrame, IProveedorService _proxy)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            proxy = _proxy;
            CargarProveedores();
        }
        
        
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string texto = txtBuscar.Text.Trim();
            var resultados = proxy.BuscarProveedores(texto);

            Proveedores.Clear();
            foreach (var u in resultados)
                Proveedores.Add(u);

        }

        private void cmbOrdenar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void dgUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void btnSeleccinar_Click(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void CargarProveedores()
        {
            try
            {
                var lista = proxy.ListarProveedores();
                    Proveedores.Clear();
                    foreach (var proveedorDto in lista)
                    {
                        Proveedores.Add(proveedorDto);
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show($"Error al cargar proveedores: {e.Message}");
                throw;
            }
        }
    }
}