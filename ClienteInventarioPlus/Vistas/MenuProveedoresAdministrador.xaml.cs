using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BibliotecaClasesNetframework.Contratos;

namespace ClienteInventarioPlus.Vistas
{
    public partial class MenuProveedoresAdministrador : UserControl
    {
        
        private Frame _mainFrame;
        private IUsuarioService usuarioService;
        public MenuProveedoresAdministrador(Frame mainFrame, IUsuarioService proveedorService)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            usuarioService = proveedorService;
        }
        
        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
           // _mainFrame.Content = new RegistrarProveedor(); 
        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnConsultar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}