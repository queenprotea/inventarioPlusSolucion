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
    public partial class ConsultarUsuarios : UserControl
    {
        private Frame _mainFrame;
        private UsuarioDTO _usuarioSesion;
        private IUsuarioService proxy;
        public ObservableCollection<UsuarioDTO> Usuarios { get; set; } = new ObservableCollection<UsuarioDTO>();

        
        public ConsultarUsuarios(Frame mainFrame, UsuarioDTO usuarioActual, IUsuarioService _proxy)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _usuarioSesion = usuarioActual;
            proxy = _proxy;
            
            this.DataContext = this; // importante para el binding
            CargarUsuarios();
        }
        
        private void CargarUsuarios()
        {
            try
            {
                var lista = proxy.ObtenerUsuarios();
                Usuarios.Clear();
                foreach (var u in lista)
                    Usuarios.Add(u);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}");
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button btn && btn.DataContext is UsuarioDTO usuario)
            {
                var resultado = MessageBox.Show($"Eliminar usuario {usuario.NombreUsuario}?", 
                    "Confirmar", MessageBoxButton.YesNo);
                if(resultado == MessageBoxResult.Yes)
                {
                    proxy.EliminarUsuario(usuario.UsuarioID); 
                    Usuarios.Remove(usuario);                  
                }
            }
        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is UsuarioDTO usuario)
            {
                _mainFrame.Content = new ModificarUsuario(_mainFrame, _usuarioSesion, proxy, usuario);
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string texto = txtBuscar.Text.Trim();
            var resultados = proxy.BuscarUsuarios(texto);

            Usuarios.Clear();
            foreach (var u in resultados)
                Usuarios.Add(u);
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Content = new UsuarioPrincipal(_mainFrame, _usuarioSesion, proxy);
        }

        private void cmbOrdenar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbOrdenar.SelectedItem is ComboBoxItem selectedItem)
            {
                string criterio = selectedItem.Content.ToString();

                List<UsuarioDTO> listaOrdenada = null;

                switch (criterio)
                {
                    case "ID":
                        listaOrdenada = Usuarios.OrderBy(u => u.UsuarioID).ToList();
                        break;
                    case "Nombre":
                        listaOrdenada = Usuarios.OrderBy(u => u.Nombre).ToList();
                        break;
                    case "Usuario":
                        listaOrdenada = Usuarios.OrderBy(u => u.NombreUsuario).ToList();
                        break;
                    case "Rol":
                        listaOrdenada = Usuarios.OrderBy(u => u.Rol).ToList();
                        break;
                }

                if (listaOrdenada != null)
                {
                    Usuarios.Clear();
                    foreach (var u in listaOrdenada)
                        Usuarios.Add(u);
                }
            }
        }
    }
}