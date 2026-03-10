using Asp.Versioning;
using Directo.Wari.Application.Features.ConsultasDocumentos.Dtos;
using Directo.Wari.Application.Features.ConsultasDocumentos.Queries.ObtenerDataCliente;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Directo.Wari.API.Controllers.V2
{
    /// <summary>
    /// Controller para Value.
    /// </summary>
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ValuesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("ConsultaDni")]
        public async Task<IActionResult> ConsultarDni([FromQuery] string Dni)
        {
            var result = await _mediator.Send(new ObtenerDataClienteQuery(1, Dni));
            if (!result.IsSuccess)
            {
                var response = new ConsultaDniResponseDto
                {
                    method = "GET",
                    status = 400, 
                    data = null,
                    error = new ConsultaErrorResponseDto
                    {
                        code = -1,
                        message = result.Error?.Message
                    }
                };

                return BadRequest(response);
            }

            return Ok(result);  
        }
    }
}
