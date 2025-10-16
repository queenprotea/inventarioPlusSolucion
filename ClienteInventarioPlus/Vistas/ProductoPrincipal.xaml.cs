using System.Windows;
using System.Windows.Controls;

namespace ClienteInventarioPlus.Vistas {
    /// <summary>
    /// Lógica de interacción para ProductoPrincipal.xaml
    /// </summary>
    public partial class ProductoPrincipal : UserControl {
        public ProductoPrincipal() {
            InitializeComponent();
        }

        private void btnRegistrarProducto_Click(object sender, RoutedEventArgs e) {
            // Esta acción requiere configuración de servicios para navegación.
            MessageBox.Show("La navegación a 'Registrar Producto' será habilitada cuando el servicio esté configurado.");
        }

        private void btnConsultarProductos_Click(object sender, RoutedEventArgs e) {
            // TODO: Aquí debes crear y navegar a tu vista para consultar productos.
            // Por ahora, se muestra un mensaje informativo.
            MessageBox.Show("La vista de consulta de productos aún no ha sido implementada.");
        }
    }
}
