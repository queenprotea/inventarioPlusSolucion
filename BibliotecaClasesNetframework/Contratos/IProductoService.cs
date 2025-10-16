using System.Collections.Generic;
using System.ServiceModel;
using BibliotecaClasesNetframework.ModelosODT;

namespace BibliotecaClasesNetframework.Contratos
{
    [ServiceContract]
    public interface IProductoService
    {
        [OperationContract]
        List<ProductoDTO> ObtenerProductos();

        [OperationContract]
        List<ProductoDTO> BuscarProductos(string valorBusqueda);

        [OperationContract]
        bool EliminarProducto(int productoID);

        [OperationContract]
        bool RegistrarProducto(ProductoDTO producto);

        [OperationContract]
        bool ActualizarProducto(ProductoDTO producto);
    }
}