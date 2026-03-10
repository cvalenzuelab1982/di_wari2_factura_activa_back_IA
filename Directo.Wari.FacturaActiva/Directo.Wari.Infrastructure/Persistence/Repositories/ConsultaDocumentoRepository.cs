using Directo.Wari.Application.Features.ConsultasDocumentos.Interfaces;
using Directo.Wari.Domain.Aggregates.ConsultaDocumentoAggregate;
using Microsoft.EntityFrameworkCore;

namespace Directo.Wari.Infrastructure.Persistence.Repositories
{
    public class ConsultaDocumentoRepository : IConsultaDocumentoRepository
    {
        private readonly ApplicationDbContext _context;

        public ConsultaDocumentoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ConsultaDocumento?> ObtenerDataCliente(int tipoDocumento, string numeroDocumento, CancellationToken cancellationToken)
        {
            return await _context.Set<ConsultaDocumento>()
            .FirstOrDefaultAsync(
                x => x.TipoDocumento == tipoDocumento &&
                     x.NumeroDocumento == numeroDocumento,
                cancellationToken);
        }
    }
}
