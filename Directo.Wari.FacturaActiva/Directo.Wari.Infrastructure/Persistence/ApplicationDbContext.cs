using Directo.Wari.Application.Common.Interfaces;
using Directo.Wari.Domain.Aggregates.ConsultaDocumentoAggregate;
using Directo.Wari.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Directo.Wari.Infrastructure.Persistence
{
    /// <summary>
    /// DbContext principal de la aplicación.
    /// </summary>
    public class ApplicationDbContext : DbContext, IApplicationDbContext, IReadDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<ConsultaDocumento> ConsultaDocumento => Set<ConsultaDocumento>();
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplica todas las configuraciones de IEntityTypeConfiguration del assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Convención: nombres de tablas en snake_case (PostgreSQL)
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(ToSnakeCase(entity.GetTableName()!));

                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(ToSnakeCase(property.GetColumnName()));
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(ToSnakeCase(key.GetName()!));
                }

                foreach (var fk in entity.GetForeignKeys())
                {
                    fk.SetConstraintName(ToSnakeCase(fk.GetConstraintName()!));
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName()!));
                }
            }

            // Fix: Owned entities comparten la PK con su owner.
            // Después de snake_case, la shadow key del owned (ej: DestinoId → destino_id)
            // debe coincidir con la columna del owner (Id → id).
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (!entity.IsOwned()) continue;

                var ownershipFk = entity.GetForeignKeys()
                    .FirstOrDefault(fk => fk.IsOwnership);

                if (ownershipFk is null) continue;

                for (var i = 0; i < ownershipFk.Properties.Count; i++)
                {
                    var principalColumn = ownershipFk.PrincipalKey.Properties[i].GetColumnName();
                    ownershipFk.Properties[i].SetColumnName(principalColumn);
                }
            }
        }

        /// <summary>
        /// IReadDbContext implementation para queries de solo lectura.
        /// </summary>
        IQueryable<T> IReadDbContext.Set<T>() => Set<T>().AsNoTracking();

        private static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var result = new System.Text.StringBuilder();
            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (char.IsUpper(c))
                {
                    if (i > 0) result.Append('_');
                    result.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }
    }
}
