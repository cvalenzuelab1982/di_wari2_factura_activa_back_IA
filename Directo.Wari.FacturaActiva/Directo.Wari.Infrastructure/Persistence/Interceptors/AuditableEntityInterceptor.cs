using Directo.Wari.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Directo.Wari.Infrastructure.Persistence.Interceptors
{
    /// <summary>
    /// Interceptor que actualiza automáticamente los campos de auditoría.
    /// </summary>
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context is not null)
            {
                UpdateAuditableEntities(eventData.Context);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void UpdateAuditableEntities(DbContext context)
        {
            var entries = context.ChangeTracker.Entries<AggregateRoot<int>>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.GetType()
                        .GetProperty(nameof(AggregateRoot<int>.CreatedAt))!
                        .SetValue(entry.Entity, DateTime.UtcNow);
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.GetType()
                        .GetProperty(nameof(AggregateRoot<int>.UpdatedAt))!
                        .SetValue(entry.Entity, DateTime.UtcNow);
                }
            }
        }
    }
}
