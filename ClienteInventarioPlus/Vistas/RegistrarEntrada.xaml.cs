using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using ClienteInventarioPlus.Utilidades;

namespace ClienteInventarioPlus.Vistas
{
    public partial class RegistrarEntrada : UserControl
    {
        private Frame _mainFrame;
        private UsuarioDTO _usuarioSesion;
        private IMovimientoService proxy;
        public ObservableCollection<ProductoDTO> Productos { get; set; } = new ObservableCollection<ProductoDTO>();

        public RegistrarEntrada(Frame mainFrame, UsuarioDTO usuarioActual, IMovimientoService _proxy)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _usuarioSesion = usuarioActual;
            proxy = _proxy;
            
            this.DataContext = this;
            CargarProductos();
        }

        
        private void CargarProductos()
        {
            try
            {
                var lista = proxy.ObtenerProductosParaMovimiento();
                Productos.Clear();
                foreach (var u in lista)
                    Productos.Add(u);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}");
            }
        }
        
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string texto = txtBuscar.Text.Trim();
            var resultados = proxy.BuscarProductosParaMovimiento(texto);

            Productos.Clear();
            foreach (var u in resultados)
                Productos.Add(u);
        }

        private void cmbOrdenar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbOrdenar.SelectedItem is ComboBoxItem selectedItem)
            {
                string criterio = selectedItem.Content.ToString();

                List<ProductoDTO> listaOrdenada = null;

                switch (criterio)
                {
                    case "ID":
                        listaOrdenada = Productos.OrderBy(u => u.ProductoID).ToList();
                        break;
                    case "Codigo":
                        listaOrdenada = Productos.OrderBy(u => u.Codigo).ToList();
                        break;
                    case "Nombre":
                        listaOrdenada = Productos.OrderBy(u => u.Nombre).ToList();
                        break;
                    case "Stock":
                        listaOrdenada = Productos.OrderBy(u => u.Stock).ToList();
                        break;
                    case "Stock apartado":
                        listaOrdenada = Productos.OrderBy(u => u.StockApartado).ToList();
                        break;
                    case "Stock minimo":
                        listaOrdenada = Productos.OrderBy(u => u.StockMinimo).ToList();
                        break;
                }

                if (listaOrdenada != null)
                {
                    Productos.Clear();
                    foreach (var u in listaOrdenada)
                        Productos.Add(u);
                }
            }
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Content = new MovimientoPrincipal(_mainFrame, _usuarioSesion, proxy);
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (EntradasValidas())
            {
                var producto = dgProductos.SelectedItem as ProductoDTO;

                if (producto == null)
                {
                    MessageBox.Show("Seleccione el producto en la tabla para el registro de entrada.");
                    return;
                }
                
                if (proxy == null)
                { 
                    MessageBox.Show("El servicio no está disponible. Inténtelo más tarde."); 
                    return;
                }
                
                if (!int.TryParse(txtCantidad.Text.Trim(), out int cantidad)) 
                { 
                    MessageBox.Show("La cantidad no es válida. Ingrese un número entero positivo."); 
                    return;
                }
                
                MovimientoDTO entradaRegistro = new MovimientoDTO();
                
                entradaRegistro.FechaHora = DateTime.Now; 
                entradaRegistro.UsuarioID = _usuarioSesion.UsuarioID; 
                entradaRegistro.ProductoID = producto.ProductoID; 
                entradaRegistro.TipoMovimiento = "Entrada"; 
                entradaRegistro.Cantidad = cantidad;
                
                bool registroMovimiento = proxy.RegistrarMovimiento(entradaRegistro); 

                if (registroMovimiento)
                {
                    MessageBox.Show("Entrada registrada exitosamente");
                    _mainFrame.Content = new MovimientoPrincipal(_mainFrame, _usuarioSesion, proxy);
                }
                else
                {
                    MessageBox.Show("Error al registrar el movimiento");
                }
            }
        }
        
        private bool EntradasValidas()
        {
            bool valido = true;
            
            string errorCantidad = ValidacionesEntrada.ValidarNumero(txtCantidad);
            
            tblockErrorCantidad.Text = errorCantidad ?? "";
            

            if ( errorCantidad != null )
                valido = false;

            return valido;
        }
        
        
        
    }
}