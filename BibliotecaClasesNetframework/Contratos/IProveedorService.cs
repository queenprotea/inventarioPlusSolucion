using System.Collections.Generic;
using System.ServiceModel;
using BibliotecaClasesNetframework.ModelosODT;

namespace BibliotecaClasesNetframework.Contratos
{
    [ServiceContract]
    public interface IProveedorService
    {
        [OperationContract]
        void AgregarProveedor(ProveedorDTO proveedor);

        [OperationContract]
        List<ProveedorDTO> ListarProveedores();

        [OperationContract]
        ProveedorDTO ObtenerProveedor(int id);

        [OperationContract]
        bool ActualizarProveedor(ProveedorDTO proveedor);

        [OperationContract]
        bool EliminarProveedor(int id);
        
        [OperationContract]
        List<ProveedorDTO> BuscarProveedores(string valorBusqueda);

        [OperationContract]
        List<ProveedorDTO> ObtenerProveedores();
    }
}