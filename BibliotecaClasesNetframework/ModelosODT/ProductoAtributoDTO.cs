namespace BibliotecaClasesNetframework.ModelosODT
{
    public class ProductoAtributoDTO
    {
        public int ProductoAtributoID { get; set; }
        public string NombreAtributo { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
    }

    public class CrearProductoAtributoDTO
    {
        public int ProductoID { get; set; }
        public string NombreAtributo { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
    }

    public class ActualizarProductoAtributoDTO
    {
        public string Valor { get; set; }
    }
}