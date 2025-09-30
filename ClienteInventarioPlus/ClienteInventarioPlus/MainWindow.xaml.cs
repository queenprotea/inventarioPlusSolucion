using System.Windows;
using BibliotecaClasesNetframework.ModelosODT;
using System.Windows.Controls;
using ClienteInventarioPlus.Vistas;

namespace ClienteInventarioPlus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        //public IAhorcadoService proxy;
        //public DuplexChannelFactory<IAhorcadoService> factory;
        
        private UsuarioDTO _usuario;

        public MainWindow()
        {
            InitializeComponent();
            //InicializarProxy();
            MainContent.Content = new IniciarSesion(this);
            
        }


        /*private void InicializarProxy()
        {
            var callbackInstance = new InstanceContext(new AhorcadoCallbackCliente(this)); // Si usas tu propia clase de callback
            factory = new DuplexChannelFactory<IAhorcadoService>(callbackInstance, "AhorcadoServiceEndpoint");
            proxy = factory.CreateChannel();
        }*/

        public void CambiarVista(UserControl nuevaVista)
        {
            MainContent.Content = nuevaVista;
        }

        

        
    }
}