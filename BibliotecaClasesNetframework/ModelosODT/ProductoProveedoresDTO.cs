using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaClasesNetframework.ModelosODT
{
    public class ProductoProveedoresDTO
    {
        public int ProductoID { get; set; }
        public int ProveedorID { get; set; }
        
        public virtual ProductoDTO Producto { get; set; }
        public virtual ProveedorDTO Proveedor { get; set; }
    }
}