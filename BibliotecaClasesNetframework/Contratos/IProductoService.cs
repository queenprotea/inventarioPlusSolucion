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
        List<ProductoDTO> BuscarProductosPorNombre(string nombre);

        [OperationContract]
        List<ProductoDTO> BuscarProductosPorCodigo(string codigo);

        [OperationContract]
        List<ProductoDTO> BuscarProductosPorCategoria(string categoria);

        [OperationContract]
        bool EliminarProducto(int productoID);

        [OperationContract]
        bool RegistrarProducto(ProductoDTO producto);

        [OperationContract]
        bool ActualizarProducto(ProductoDTO producto);
    }
}