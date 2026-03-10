using Directo.Wari.Application.Features.ConsultasDocumentos.Dtos;
using Directo.Wari.Application.Features.HttpUtil.Interfaces;
using MediatR;

namespace Directo.Wari.Application.Features.HttpUtil.Queries
{
    public class ConsultaDniHandler : IRequestHandler<ConsultaDniQuery, ConsultaDniResponseDto>
    {
        private readonly IHttpUtilRepository _repository;

        public ConsultaDniHandler(IHttpUtilRepository repository)
        {
            _repository = repository;
        }

        public async Task<ConsultaDniResponseDto> Handle(ConsultaDniQuery request, CancellationToken cancellationToken)
        {
            return await _repository.ConsultaDni(request.Dni);
        }
    }
}
