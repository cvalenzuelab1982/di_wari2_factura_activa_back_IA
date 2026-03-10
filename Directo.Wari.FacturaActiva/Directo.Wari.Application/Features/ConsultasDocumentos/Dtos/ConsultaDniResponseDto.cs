namespace Directo.Wari.Application.Features.ConsultasDocumentos.Dtos
{
    public class ConsultaDniResponseDto
    {
        public string? method { get; set; }
        public int status { get; set; }
        public ConsultaDniDataResponseDto? data { get; set; }
        public ConsultaErrorResponseDto? error { get; set; }    
    }

    public class ConsultaDniDataResponseDto
    {
        public string? dni { get; set; }
        public string? nombres { get; set; }
        public string? apellidoPaterno { get; set; }
        public string? apellidoMaterno { get; set; }
        public string? nombreCompleto { get; set; }
    }

    public class ConsultaErrorResponseDto
    {
        public int code { get; set; }
        public string? message { get; set; }
    }
}
