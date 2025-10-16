using System.ServiceModel;

namespace BibliotecaClasesNetframework.Contratos
{
    [ServiceContract]
    public interface IReporteService
    {
        [OperationContract]
        string GenerarReporteProductos(string rutaTemporal);

        [OperationContract]
        string GenerarReporteMovimientos(string rutaTemporal, string fechaInicio, string fechaFin);
    }
}