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
    public partial class HistorialMovimientos : UserControl
    {
        private Frame _mainFrame;
        private UsuarioDTO _usuarioSesion;
        private IMovimientoService proxy;
        public ObservableCollection<MovimientoDTO> movimientos { get; set; } = new ObservableCollection<MovimientoDTO>();

        
        public HistorialMovimientos(Frame mainFrame, UsuarioDTO usuarioActual, IMovimientoService _proxy)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _usuarioSesion = usuarioActual;
            proxy = _proxy;
            
            this.DataContext = this; // importante para el binding
            CargarMovimientos();
        }
        
        private void CargarMovimientos()
        {
            try
            {
                var lista = proxy.ObtenerMovimientos();
                movimientos.Clear();
                foreach (var u in lista)
                    movimientos.Add(u);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar movimientos: {ex.Message}");
            }
        }
        

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string texto = txtBuscar.Text.Trim();
            var resultados = proxy.BuscarMovimientos(texto);

            movimientos.Clear();
            foreach (var u in resultados)
                movimientos.Add(u);
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Content = new MovimientoPrincipal(_mainFrame, _usuarioSesion, proxy);
        } 

        private void cmbOrdenar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbOrdenar.SelectedItem is ComboBoxItem selectedItem)
            {
                string criterio = selectedItem.Content.ToString();

                List<MovimientoDTO> listaOrdenada = null;

                switch (criterio)
                {
                    case "ID":
                        listaOrdenada = movimientos.OrderBy(u => u.UsuarioID).ToList();
                        break;
                    case "FechaHora":
                        listaOrdenada = movimientos.OrderBy(u => u.FechaHora).ToList();
                        break;
                    case "Tipo de movimiento":
                        listaOrdenada = movimientos.OrderBy(u => u.TipoMovimiento).ToList();
                        break;
                    case "Cantidad":
                        listaOrdenada = movimientos.OrderBy(u => u.Cantidad).ToList();
                        break;
                }

                if (listaOrdenada != null)
                {
                    movimientos.Clear();
                    foreach (var u in listaOrdenada)
                        movimientos.Add(u);
                }
            }
        }

        private void btnAnular_Click(object sender, RoutedEventArgs e)
        {
            var movimiento = dgMovimientos.SelectedItem as MovimientoDTO;

            if (movimiento == null)
            {
                MessageBox.Show("Seleccione el movimiento en la tabla para Anular.");
                return;
            }
              
            var confirmacion = MessageBox.Show(
                "¿Está seguro que desea anular este movimiento?\n\nEsta acción no se puede deshacer.",
                "Confirmar anulación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (confirmacion != MessageBoxResult.Yes)
            {
                // Si el usuario cancela, no hace nada
                MessageBox.Show("Operación cancelada.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            if (proxy == null)
            { 
                MessageBox.Show("El servicio no está disponible. Inténtelo más tarde."); 
                return;
            }
                
            bool anulacionMovimiento = proxy.AnularMovimiento(movimiento); 

            if (anulacionMovimiento)
            {
                MessageBox.Show("Movimiento anulado correctamente");
                CargarMovimientos();
            }
            else
            {
                MessageBox.Show("Error al anular el movimiento");
            }
        }
    }
}