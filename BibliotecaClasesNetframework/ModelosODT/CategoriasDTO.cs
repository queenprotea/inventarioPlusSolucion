using System.ComponentModel.DataAnnotations;

namespace BibliotecaClasesNetframework.ModelosODT
{
    public class CategoriasDTO
    {
        [Key]
        public string IDCategoria { get; set; }
        public string Nombre  { get; set; } = string.Empty;
    }
}