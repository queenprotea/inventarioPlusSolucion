using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServidorInventarioPlus.Modelos
{
    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        public int IDCategoria { get; set; }
        public string Nombre { get; set; } = string.Empty;
        
    }
}