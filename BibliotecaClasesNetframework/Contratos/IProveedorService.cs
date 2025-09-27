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
        void ActualizarProveedor(ProveedorDTO proveedor);

        [OperationContract]
        void EliminarProveedor(int id);
    }
}