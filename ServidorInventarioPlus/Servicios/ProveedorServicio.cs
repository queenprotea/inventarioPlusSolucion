using System.Collections.Generic;
using BibliotecaClasesNetframework.ModelosODT;
using BibliotecaClasesNetframework.Contratos;
using ServidorInventarioPlus.Modelos;

namespace ServidorInventarioPlus.Servicios
{
    public class ProveedorService
    {
        private readonly InventarioPlusContext _context;

        public ProveedorService()
        {
            _context = new InventarioPlusContext();
        }

        // Listar todos los proveedores
        public List<Proveedor> ObtenerProveedores()
        {
            return _context.Proveedores.ToList();
        }

        // Obtener proveedor por ID
        public Proveedor ObtenerProveedor(int id)
        {
            return _context.Proveedores.Find(id);
        }

        // Agregar nuevo proveedor
        public void AgregarProveedor(Proveedor proveedor)
        {
            _context.Proveedores.Add(proveedor);
            _context.SaveChanges();
        }

        // Actualizar proveedor
        public void ActualizarProveedor(Proveedor proveedor)
        {
            var existente = _context.Proveedores.Find(proveedor.ProveedorID);
            if (existente != null)
            {
                existente.Nombre = proveedor.Nombre;
                existente.Telefono = proveedor.Telefono;
                existente.Correo = proveedor.Correo;
                existente.Direccion = proveedor.Direccion;
                existente.Categoria = proveedor.Categoria;
                _context.SaveChanges();
            }
        }

        // Eliminar proveedor
        public void EliminarProveedor(int id)
        {
            var proveedor = _context.Proveedores.Find(id);
            if (proveedor != null)
            {
                _context.Proveedores.Remove(proveedor);
                _context.SaveChanges();
            }
        }
    }
}