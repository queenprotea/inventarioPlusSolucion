using System.Collections.Generic;
using BibliotecaClasesNetframework.ModelosODT;
using BibliotecaClasesNetframework.Contratos;
namespace ServidorInventarioPlus.Servicios
{
    public class ProveedorService : IProveedorService
    {
        private static List<ProveedorDTO> _proveedores = new List<ProveedorDTO>();

        public void AgregarProveedor(ProveedorDTO proveedor)
        {
            proveedor.ProveedorID = _proveedores.Count + 1;
            _proveedores.Add(proveedor);
        }

        public List<ProveedorDTO> ListarProveedores()
        {
            return _proveedores;
        }

        public ProveedorDTO ObtenerProveedor(int id)
        {
            return _proveedores.Find(p => p.ProveedorID == id);
        }

        public void ActualizarProveedor(ProveedorDTO proveedor)
        {
            var index = _proveedores.FindIndex(p => p.ProveedorID == proveedor.ProveedorID);
            if (index != -1)
                _proveedores[index] = proveedor;
        }

        public void EliminarProveedor(int id)
        {
            _proveedores.RemoveAll(p => p.ProveedorID == id);
        }
    }
}