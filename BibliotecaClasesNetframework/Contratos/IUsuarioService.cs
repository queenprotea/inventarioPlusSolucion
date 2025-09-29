using System.ServiceModel;
using BibliotecaClasesNetframework.ModelosODT;

namespace BibliotecaClasesNetframework.Contratos
{
    [ServiceContract]
    public interface IUsuarioService
    {
        [OperationContract]
        UsuarioDTO IniciarSesion(string NombreUsuario, string Contrasena);
        
        [OperationContract]
        bool RegistrarUsuario(UsuarioDTO usuario);
    }
}