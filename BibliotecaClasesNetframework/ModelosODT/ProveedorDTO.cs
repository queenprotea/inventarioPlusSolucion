namespace BibliotecaClasesNetframework.ModelosODT
{
    public class ProveedorDTO
    {
        
        public int ProveedorID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public string Categoria { get; set; }
    }

    public class CrearProveedorDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
    }

    public class ActualizarProveedorDTO
    {
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public string Nombre { get; set; }
    }
    

}