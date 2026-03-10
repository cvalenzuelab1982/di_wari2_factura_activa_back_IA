using Directo.Wari.Domain.Aggregates.ConsultaDocumentoAggregate;

namespace Directo.Wari.Application.Features.ConsultasDocumentos.Interfaces
{
    public interface IConsultaDocumentoRepository
    {
        Task<ConsultaDocumento?> ObtenerDataCliente(int tipoDocumento, string numeroDocumento, CancellationToken cancellationToken);
    }
}
