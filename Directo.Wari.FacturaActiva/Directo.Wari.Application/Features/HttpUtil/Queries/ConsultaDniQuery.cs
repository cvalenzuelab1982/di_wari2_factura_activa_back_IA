using Directo.Wari.Application.Features.ConsultasDocumentos.Dtos;
using MediatR;

namespace Directo.Wari.Application.Features.HttpUtil.Queries
{
    public sealed record ConsultaDniQuery(string Dni) : IRequest<ConsultaDniResponseDto>;
}
