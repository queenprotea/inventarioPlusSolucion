using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace ClienteInventarioPlus.Vistas {
    public partial class ModificarProductoVista : UserControl {
        private readonly IProductoService _proxyProducto;
        private readonly ProductoDTO _productoAModificar; // Almacena el producto que estamos editando
        private ObservableCollection<ProveedorDTO> _proveedoresProducto;
        private readonly IProveedorService _proxyProveedor;
        private readonly string _modo;
        private Frame _mainFrame;

        public ModificarProductoVista(ProductoDTO producto, IProductoService proxyProducto, IProveedorService proxyProveedor, string modo, Frame mainFrame = null) {
            InitializeComponent();
            _productoAModificar = producto;
            _proxyProducto = proxyProducto;
            _proxyProveedor = proxyProveedor;
            _proveedoresProducto = new ObservableCollection<ProveedorDTO>(producto.proveedores);
            _modo = modo;
            _mainFrame = mainFrame;
            if (_modo == "consultar")
                ConfigurarModoConsulta();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            CargarDatosDelProducto();
            CargarCategorias();
            CargarProveedores();
            CmbProveedores.ItemsSource = _proxyProveedor.ObtenerProveedores();
            CmbProveedores.DisplayMemberPath  = "Nombre";
            
        }

        private void CargarDatosDelProducto() {
            // Rellena cada campo con la información del producto recibido
            TxtNombreProducto.Text = _productoAModificar.Nombre;
            TxtCodigo.Text = _productoAModificar.Codigo; 
            TxtDescripcion.Text = _productoAModificar.Descripcion;
            TxtStockActual.Text = _productoAModificar.Stock.ToString();
            TxtStockMinimo.Text = _productoAModificar.StockMinimo.ToString();
            TxtPrecioCompra.Text =  _productoAModificar.PrecioCompra.ToString();
            TxtPrecioVenta.Text =  _productoAModificar.PrecioVenta.ToString();
        }

        private void CargarProveedores()
        {
            try {
                // Llama al servicio para obtener la lista de todos los proveedores
                DgProveedores.ItemsSource = _proveedoresProducto;
            }
            catch (Exception ex) {
                MessageBox.Show($"Error al cargar los proveedores: {ex.Message}", "Error de Servicio", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CargarCategorias()
        {
            try
            {
                // Llama al servicio para obtener las categorías
                var categorias = _proxyProducto.ObtenerCategorias();
                CmbCategoria.ItemsSource = categorias;
                CmbCategoria.DisplayMemberPath = "Nombre"; // La propiedad del objeto a mostrar
                CmbCategoria.SelectedValuePath = "IDCategoria"; // El valor a usar internamente
                CmbCategoria.SelectedValue = _productoAModificar.IDCategoria;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las categorías: {ex.Message}", "Error de Servicio", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e) {
            // --- 1. Validar Entradas ---
            if (string.IsNullOrWhiteSpace(TxtNombreProducto.Text)) {
                MessageBox.Show("El nombre del producto no puede estar vacío.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // --- 2. Actualizar el Objeto DTO con los nuevos valores ---
            try {
                _productoAModificar.Nombre = TxtNombreProducto.Text;
                _productoAModificar.Codigo = TxtCodigo.Text;
                _productoAModificar.Descripcion = TxtDescripcion.Text;
                _productoAModificar.Stock = int.Parse(TxtStockActual.Text);
                _productoAModificar.StockMinimo = int.Parse(TxtStockMinimo.Text);
                _productoAModificar.PrecioCompra = decimal.Parse(TxtPrecioCompra.Text);
                _productoAModificar.PrecioVenta = decimal.Parse(TxtPrecioVenta.Text);

                var proveedoresNuevos = DgProveedores.Items.OfType<ProveedorDTO>().ToList();
                _productoAModificar.proveedores = proveedoresNuevos;
            }
            catch (FormatException) {
                MessageBox.Show("Por favor, asegúrese de que los campos numéricos (stock) sean correctos.", "Error de Formato", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // --- 3. Llamar al Servicio para Actualizar ---
            try {
                bool resultado = _proxyProducto.ActualizarProducto(_productoAModificar);
                if (resultado) {
                    MessageBox.Show("Producto actualizado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Regresar a la vista anterior de forma segura
                    var nav = System.Windows.Navigation.NavigationService.GetNavigationService(this);
                    nav?.GoBack();
                }
                else {
                    MessageBox.Show("No se pudo actualizar el producto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Error al conectar con el servicio: {ex.Message}", "Error de Servicio", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) {
            if (_mainFrame != null)
            {
                _mainFrame.Content = new ConsultarProductoAdmin(_proxyProducto, _proxyProveedor, _mainFrame);
            }
            else
            {
                // Regresar a la pantalla anterior (la lista de productos)
                var nav = System.Windows.Navigation.NavigationService.GetNavigationService(this);
                nav?.GoBack();
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            var proveedorSeleccionado = DgProveedores.SelectedItem as ProveedorDTO;

            if (proveedorSeleccionado == null)
            {
                MessageBox.Show("Selecciona un proveedor para eliminar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var confirmar = MessageBox.Show($"¿Eliminar al proveedor '{proveedorSeleccionado.Nombre}'?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmar == MessageBoxResult.Yes)
            {
                _proveedoresProducto.Remove(proveedorSeleccionado);
            }
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            ProveedorDTO seleccionado = CmbProveedores.SelectedItem as ProveedorDTO;

            if (seleccionado != null)
            {
                // Verificamos que no se repita
                if (!_productoAModificar.proveedores.Contains(seleccionado))
                {
                    _proveedoresProducto.Add(seleccionado);
                }
                else
                {
                    MessageBox.Show("Este proveedor ya fue agregado.");
                }
            }
            else
            {
                MessageBox.Show("Selecciona un proveedor antes de agregar.");
            }
        }
        
        private void ConfigurarModoConsulta() {
            // Deshabilitar campos de texto
            TxtNombreProducto.IsReadOnly = true;
            TxtCodigo.IsReadOnly = true;
            TxtDescripcion.IsReadOnly = true;
            TxtStockActual.IsReadOnly = true;
            TxtStockMinimo.IsReadOnly = true;
            TxtPrecioCompra.IsReadOnly = true;
            TxtPrecioVenta.IsReadOnly = true;
            // Deshabilitar combobox
            CmbCategoria.IsEnabled = false;
            CmbProveedores.Visibility = Visibility.Collapsed;
            // Ocultar botones
            BtnAgregar.Visibility = Visibility.Collapsed;
            BtnEliminar.Visibility = Visibility.Collapsed;
            BtnGuardar.Visibility = Visibility.Collapsed;
        }
        
    }
}
