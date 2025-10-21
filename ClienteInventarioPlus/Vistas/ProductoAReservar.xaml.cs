using System;
using System.Windows;
using System.Windows.Controls;
using BibliotecaClasesNetframework.Contratos;
using BibliotecaClasesNetframework.ModelosODT;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace ClienteInventarioPlus.Vistas {
    public partial class ProductoAReservarVista : UserControl {
        private Frame _mainFrame;
        private readonly IReservaService _proxyReserva;
        private readonly IProductoService _proxyProducto;
        private readonly ProductoDTO _productoAReservar;

        public ProductoAReservarVista(ProductoDTO producto, IReservaService proxyReserva, IProductoService proxyproducto, Frame frame ) {
            InitializeComponent();
            _productoAReservar = producto;
            _proxyReserva = proxyReserva;
            _proxyReserva = proxyReserva;
            _proxyProducto = proxyproducto;
            _mainFrame = frame;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            // Carga los datos del producto en los TextBlocks de solo lectura
            TbNombre.Text = _productoAReservar.Nombre;
            TbCodigo.Text = _productoAReservar.Codigo;
            TbDescripcion.Text = _productoAReservar.Descripcion;
            TbStock.Text = _productoAReservar.Stock.ToString();
            TbPrecio.Text = _productoAReservar.PrecioVenta.ToString();
            TbCategoria.Text = _productoAReservar.NombreCategoria;
        }

        private void BtnReservar_Click(object sender, RoutedEventArgs e) {
            // --- 1. Validar Entradas ---
            if (!int.TryParse(TxtCantidad.Text, out int cantidad) || cantidad <= 0) {
                MessageBox.Show("Por favor, ingrese una cantidad numérica válida y mayor a cero.", "Cantidad Inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cantidad > _productoAReservar.Stock) {
                MessageBox.Show("No puede reservar más unidades de las que hay en stock.", "Stock Insuficiente", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // --- 2. Crear DTO y Llamar al Servicio ---
            try {
                var nuevaReserva = new ReservaDTO {
                    ProductoID = _productoAReservar.ProductoID,
                    CantidadReservada = cantidad,
                    NumeroReserva = $"RSV-{DateTime.Now:yyyyMMddHHmmss}",
                    Cliente = TxtNombreCliente.Text
                };

                bool resultado = _proxyReserva.CrearReserva(nuevaReserva);

                if (resultado) {
                    MessageBox.Show("Reserva creada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Regresar a la pantalla anterior de forma segura
                    _mainFrame.Content = new ReservarProductoVista(_proxyReserva, _proxyProducto, _mainFrame);
                }
                else {
                    MessageBox.Show("No se pudo crear la reserva.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Error al procesar la reserva: {ex.Message}", "Error de Servicio", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) {
            // Regresa a la pantalla anterior (la lista de productos)
            var nav = System.Windows.Navigation.NavigationService.GetNavigationService(this);
            nav?.GoBack();
        }
    }
}