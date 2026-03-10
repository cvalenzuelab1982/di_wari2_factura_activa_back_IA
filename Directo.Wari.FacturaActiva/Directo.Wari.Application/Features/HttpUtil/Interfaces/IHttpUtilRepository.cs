using Directo.Wari.Application.Features.ConsultasDocumentos.Dtos;

namespace Directo.Wari.Application.Features.HttpUtil.Interfaces
{
    public interface IHttpUtilRepository
    {
        Task<ConsultaDniResponseDto?> ConsultaDni(string dni);
    }
}
