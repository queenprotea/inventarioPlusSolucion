using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;

namespace ClienteInventarioPlus.Vistas {
    /// <summary>
    /// Lógica de interacción para ProductoPrincipal.xaml
    /// </summary>
    public partial class ProductoPrincipal : UserControl {
        private Frame _mainFrame;
        private readonly IProductoService _proxyProducto;
        private readonly IProveedorService _proxyProveedor;
        public ProductoPrincipal(Frame mainFrame, IProductoService proxyProducto, IProveedorService proxyProveedor) {
            InitializeComponent();
            _mainFrame = mainFrame;
            _proxyProducto = proxyProducto;
            _proxyProveedor = proxyProveedor;
            
        }

        private void btnRegistrarProducto_Click(object sender, RoutedEventArgs e) {
            // Esta acción requiere configuración de servicios para navegación.
            _mainFrame.Content = new RegistrarProductoVista(_mainFrame, _proxyProducto, _proxyProveedor);
        }

        private void btnConsultarProductos_Click(object sender, RoutedEventArgs e) {
            // TODO: Aquí debes crear y navegar a tu vista para consultar productos.
            // Por ahora, se muestra un mensaje informativo.
            MessageBox.Show("La vista de consulta de productos aún no ha sido implementada.");
        }
    }
}
