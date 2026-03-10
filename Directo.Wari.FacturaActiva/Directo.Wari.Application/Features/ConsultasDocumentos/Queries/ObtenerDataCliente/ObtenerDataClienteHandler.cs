using Directo.Wari.Application.Common.Models;
using Directo.Wari.Application.Features.ConsultasDocumentos.Dtos;
using Directo.Wari.Application.Features.ConsultasDocumentos.Interfaces;
using Directo.Wari.Application.Features.HttpUtil.Interfaces;
using Directo.Wari.Domain.Aggregates.ConsultaDocumentoAggregate;
using Directo.Wari.Domain.Interfaces;
using MediatR;
using System.Text.Json;

namespace Directo.Wari.Application.Features.ConsultasDocumentos.Queries.ObtenerDataCliente
{
    public class ObtenerDataClienteHandler : IRequestHandler<ObtenerDataClienteQuery, Result<ConsultaDniDataResponseDto>>
    {
        private readonly IConsultaDocumentoRepository _ConsultaDocumentoRepository;
        private readonly IHttpUtilRepository _HttpUtilRepository;
        private readonly IRepository<ConsultaDocumento> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ObtenerDataClienteHandler(IConsultaDocumentoRepository consultaDocumentoRepository, IHttpUtilRepository httpUtilRepository, IRepository<ConsultaDocumento> repository, IUnitOfWork unitOfWork)
        {
            _ConsultaDocumentoRepository = consultaDocumentoRepository;
            _HttpUtilRepository = httpUtilRepository;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ConsultaDniDataResponseDto>> Handle(ObtenerDataClienteQuery request, CancellationToken cancellationToken)
        {
            var data = await _ConsultaDocumentoRepository.ObtenerDataCliente(request.tipoDocumento, request.numeroDocumento, cancellationToken);

            //Si existe en BD
            if (data != null || !string.IsNullOrWhiteSpace(data?.NombreCompleto))
            {
                var dto = new ConsultaDniDataResponseDto
                {
                    nombreCompleto = data?.NombreCompleto,
                    apellidoMaterno = data?.ApellidoMaterno,
                    apellidoPaterno = data?.ApellidoPaterno,
                    dni = data?.NumeroDocumento,
                    nombres = data?.Nombres
                };

                return Result.Success(dto);
            }

            //Si no existe en BD, consultar servicio externo
            var resultadoExterno = await _HttpUtilRepository.ConsultaDni(request.numeroDocumento);

            if (resultadoExterno == null || resultadoExterno.data == null)
            {
                return Result.Failure<ConsultaDniDataResponseDto>(Error.Validation("Consulta cliente al servicio externo, no encontrado"));
            }

            string estadoConsulta = "ERROR";
            int status = (resultadoExterno != null ? resultadoExterno.status : 0);

            if (status == 200)
            {
                estadoConsulta = "OK";
            }
            else if (status == 400)
            {
                estadoConsulta = "NO_ENCONTRADO";
            }

            //Guardar cliente consultado en BD
            var entidad = ConsultaDocumento.Create(
                request.tipoDocumento,
                request.numeroDocumento,
                estadoConsulta,
                DateTime.UtcNow,
                "",
                "",
                "",
                "",
                "",
                resultadoExterno?.data.nombres!,
                resultadoExterno?.data.apellidoPaterno!,
                resultadoExterno?.data.apellidoMaterno!,
                resultadoExterno?.data.nombreCompleto!,
                JsonSerializer.Serialize(resultadoExterno));

            await _repository.AddAsync(entidad, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dtoNuevo = new ConsultaDniDataResponseDto
            {
                nombreCompleto = resultadoExterno?.data.nombreCompleto,
                apellidoMaterno = resultadoExterno?.data.apellidoMaterno,
                apellidoPaterno = resultadoExterno?.data.apellidoPaterno,
                dni = resultadoExterno?.data.dni,
                nombres = resultadoExterno?.data.nombres
            };

            return Result.Success(dtoNuevo);
        }

    }
}
