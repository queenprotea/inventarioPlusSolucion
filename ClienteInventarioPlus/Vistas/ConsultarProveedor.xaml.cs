using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private string _modo;
        public ObservableCollection<ProveedorDTO> Proveedores { get; set; } = new ObservableCollection<ProveedorDTO>();
        public ConsultarProveedor(Frame mainFrame, IProveedorService _proxy, String modo)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            proxy = _proxy;
            this.DataContext = this;
            _modo = modo;
            
            CargarProveedores();
            ConfigurarVista();
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
            if (cmbOrdenar.SelectedItem is ComboBoxItem selectedItem)
            {
                string criterio = selectedItem.Content.ToString();

                List<ProveedorDTO> listaOrdenada = null;

                switch (criterio)
                {
                    case "Correo":
                        listaOrdenada = Proveedores.OrderBy(u => u.Correo).ToList();
                        break;
                    case "Nombre":
                        listaOrdenada = Proveedores.OrderBy(u => u.Nombre).ToList();
                        break;
                    case "Direccion":
                        listaOrdenada = Proveedores.OrderBy(u => u.Direccion).ToList();
                        break;
                    case "Telefono":
                        listaOrdenada = Proveedores.OrderBy(u => u.Telefono).ToList();
                        break;
                }

                if (listaOrdenada != null)
                {
                    Proveedores.Clear();
                    foreach (var u in listaOrdenada)
                        Proveedores.Add(u);
                }
            }
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
          
            if (dgProveedores.SelectedItem is ProveedorDTO proveedor)
            {
                switch (_modo)
                {
                    case "consultar":
                        MessageBox.Show(
                            $"Nombre: {proveedor.Nombre}\n" +
                            $"Correo: {proveedor.Correo}\n" +
                            $"Dirección: {proveedor.Direccion}\n" +
                            $"Teléfono: {proveedor.Telefono}",
                            "Proveedor Seleccionado",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                        );
                        break; 
                    case "eliminar":
                        var confirmar = MessageBox.Show(
                            $"¿Seguro que deseas eliminar al proveedor {proveedor.Nombre}?",
                            "Confirmar eliminación",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning
                        );

                        if (confirmar == MessageBoxResult.Yes)
                        {
                            proxy.EliminarProveedor(proveedor.ProveedorID);
                            CargarProveedores();
                            MessageBox.Show("Proveedor eliminado con éxito.");
                        }
                        break;
                    case "modificar":
                        _mainFrame.Content = new ModificarProveedor(_mainFrame, proxy, proveedor, _modo);
                        break;
                }
            }
            else
            {
                MessageBox.Show("Por favor selecciona un proveedor de la lista.");
            }
        }

        private void CargarProveedores()
        {
            try
            {
                var lista = proxy.ObtenerProveedores();
                    Proveedores.Clear();
                    foreach (var p in lista)
                    {
                        Proveedores.Add(p);
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show($"Error al cargar proveedores: {e.Message}");
                throw;
            }
        }
        
        private void ConfigurarVista()
        {
            // Cambiar el texto del botón según el modo
            switch (_modo)
            {
                case "consultar":
                    btnSeleccionar.Content = "Seleccionar";
                    break; 
                case "eliminar":
                    btnSeleccionar.Content = "Eliminar";
                    break;
                case "modificar":
                    btnSeleccionar.Content = "Modificar";
                    break;
            }
        }
        
    }
}