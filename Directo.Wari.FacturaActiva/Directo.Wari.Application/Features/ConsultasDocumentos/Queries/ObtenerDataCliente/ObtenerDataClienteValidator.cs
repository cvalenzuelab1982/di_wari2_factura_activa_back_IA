using FluentValidation;

namespace Directo.Wari.Application.Features.ConsultasDocumentos.Queries.ObtenerDataCliente
{
    public class ObtenerDataClienteValidator : AbstractValidator<ObtenerDataClienteQuery>
    {
        public ObtenerDataClienteValidator()
        {
            Console.WriteLine("Validator ejecutado");

            RuleFor(x => x.numeroDocumento)
                    .NotEmpty()
                    .WithMessage("Numero de documento es requerido")
                    .Matches(@"^\d+$")
                    .WithMessage("El número de documento solo debe contener números.")
                    .Length(8)
                    .WithMessage("El número de documento debe tener exactamente 8 dígitos.");
        }
    }
}
