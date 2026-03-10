using Directo.Wari.Domain.Common;

namespace Directo.Wari.Domain.Aggregates.ConsultaDocumentoAggregate
{
    public class ConsultaDocumento : AggregateRoot<int>
    {
        public int TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; } = string.Empty;
        public string? EstadoConsulta { get; set; }
        public DateTime? FechaConsulta { get; set; }
        public string? RazonSocial { get; set; }
        public string? DireccionFiscal { get; set; }
        public string? EstadoContribuyente { get; set; }
        public string? CondicionDomicilio { get; set; }
        public string? Ubigeo { get; set; }
        public string? Nombres { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? NombreCompleto { get; set; }
        public string? JsonRespuesta { get; set; }

        public ConsultaDocumento() { } // EF Core

        public static ConsultaDocumento Create(
            int tipoDocumento,
            string numeroDocumento,
            string estadoConsulta,
            DateTime fechaConsulta,
            string razonSocial,
            string direccionFiscal,
            string estadoContribuyente,
            string condicionDomicilio,
            string ubigeo,
            string nombres,
            string apellidoPaterno,
            string apellidoMaterno,
            string nombreCompleto,
            string jsonRespuesta
            )
        {
            return new ConsultaDocumento
            {
                TipoDocumento = tipoDocumento,
                NumeroDocumento = numeroDocumento,
                EstadoConsulta = estadoConsulta,
                FechaConsulta = fechaConsulta,
                RazonSocial = razonSocial,
                DireccionFiscal = direccionFiscal,
                EstadoContribuyente = estadoContribuyente,
                CondicionDomicilio = condicionDomicilio,
                Ubigeo = ubigeo,
                Nombres = nombres,
                ApellidoPaterno = apellidoMaterno,
                ApellidoMaterno = apellidoMaterno,
                NombreCompleto = nombreCompleto,
                JsonRespuesta = jsonRespuesta
            };
        }
    }
}
