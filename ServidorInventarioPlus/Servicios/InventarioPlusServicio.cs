using System.ServiceModel;

namespace ServidorInventarioPlus.Servicios

{
    [ServiceBehavior(
               InstanceContextMode = InstanceContextMode.Single, // Una sola instancia del servicio
               ConcurrencyMode = ConcurrencyMode.Single          // Procesa una petición a la vez
           )] 
    public class InventarioPlusServicio
    {
       
    }
}