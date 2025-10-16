using System.ServiceModel;
using BibliotecaClasesNetframework.ModelosODT;

namespace BibliotecaClasesNetframework.Contratos
{
    public interface IProductiService
    {
        [OperationContract]
       bool RegistarProducto(ProductoDTO producto);
       
       
    }
}