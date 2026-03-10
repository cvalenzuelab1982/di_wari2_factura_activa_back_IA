using Directo.Wari.Application.Common.Models;
using Directo.Wari.Application.Features.ConsultasDocumentos.Dtos;
using MediatR;

namespace Directo.Wari.Application.Features.ConsultasDocumentos.Queries.ObtenerDataCliente
{
    public sealed record ObtenerDataClienteQuery(int tipoDocumento, string numeroDocumento) : IRequest<Result<ConsultaDniDataResponseDto>>;
}
