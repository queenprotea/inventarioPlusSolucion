using System.Collections.Generic;
using System.ServiceModel;
using BibliotecaClasesNetframework.ModelosODT;

namespace BibliotecaClasesNetframework.Contratos
{
    [ServiceContract]
    public interface IReservaService
    {
        [OperationContract]
        bool CrearReserva(ReservaDTO reserva);

        [OperationContract]
        List<ReservaDTO> ObtenerReservas();

        [OperationContract]
        bool CancelarReserva(int reservaID);
    }
}