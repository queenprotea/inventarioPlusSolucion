using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServidorInventarioPlus.Modelos
{
    [Table("ProductoProveedores")]
    public class ProductoProveedores
    {
        [Key, Column(Order = 0)]
        public int ProductoID { get; set; }
        public virtual Producto Producto { get; set; }
        [Key, Column(Order = 1)]
        public int ProveedorID { get; set; }
        public virtual Proveedor Proveedor { get; set; }
    }
}