using System;
using System.ServiceModel;
using BibliotecaClasesNetframework.Contratos;
using ServidorInventarioPlus.Servicios;

namespace ServidorInventarioPlus
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(ProveedorService),
                       new Uri("http://localhost:8080/Proveedores")))
            {
                host.AddServiceEndpoint(typeof(IProveedorService), new BasicHttpBinding(), "");
                host.Open();

                Console.WriteLine("✅ Servicio de Proveedores corriendo en http://localhost:8080/Proveedores");
                Console.WriteLine("Presiona ENTER para salir...");
                Console.ReadLine();
            }
        }
    }
}