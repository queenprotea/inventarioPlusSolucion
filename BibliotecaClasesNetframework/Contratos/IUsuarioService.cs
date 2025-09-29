using System.Collections.Generic;
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
        
        [OperationContract]
        List<UsuarioDTO> ObtenerUsuarios();

        [OperationContract]
        List<UsuarioDTO> BuscarUsuarios(string valorBusqueda);

        [OperationContract]
        bool EliminarUsuario(int usuarioId);

        [OperationContract]
        bool ModificarUsuario(UsuarioDTO usuario);

    }
}