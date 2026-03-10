using Directo.Wari.Application.Common.Interfaces;

namespace Directo.Wari.Infrastructure.Services
{
    /// <summary>
    /// Proveedor de fecha/hora del sistema.
    /// </summary>
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
