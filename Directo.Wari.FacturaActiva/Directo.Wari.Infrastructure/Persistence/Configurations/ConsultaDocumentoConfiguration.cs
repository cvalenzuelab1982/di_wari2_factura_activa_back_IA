using Directo.Wari.Domain.Aggregates.ConsultaDocumentoAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Directo.Wari.Infrastructure.Persistence.Configurations
{
    public class ConsultaDocumentoConfiguration : IEntityTypeConfiguration<ConsultaDocumento>
    {
        public void Configure(EntityTypeBuilder<ConsultaDocumento> builder)
        {
            builder.ToTable("consulta_documento");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id_consulta_documento")
                .UseIdentityColumn();

            builder.Property(x => x.TipoDocumento)
                .HasColumnName("tipo_documento")
                .IsRequired();

            builder.Property(x => x.NumeroDocumento)
                .HasColumnName("numero_documento")
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(x => x.EstadoConsulta)
                .HasColumnName("estado_consulta")
                .HasMaxLength(20);

            builder.Property(x => x.FechaConsulta)
                .HasColumnName("fecha_consulta");

            builder.Property(x => x.RazonSocial)
                .HasColumnName("razon_social")
                .HasMaxLength(100);

            builder.Property(x => x.DireccionFiscal)
                .HasColumnName("direccion_fiscal")
                .HasMaxLength(255);

            builder.Property(x => x.EstadoContribuyente)
                .HasColumnName("estado_contribuyente")
                .HasMaxLength(20);

            builder.Property(x => x.CondicionDomicilio)
                .HasColumnName("condicion_domicilio")
                .HasMaxLength(20);

            builder.Property(x => x.Ubigeo)
                .HasColumnName("ubigeo")
                .HasMaxLength(6);

            builder.Property(x => x.Nombres)
                .HasColumnName("nombres")
                .HasMaxLength(60);

            builder.Property(x => x.ApellidoPaterno)
                .HasColumnName("apellido_paterno")
                .HasMaxLength(20);

            builder.Property(x => x.ApellidoMaterno)
                .HasColumnName("apellido_materno")
                .HasMaxLength(20);

            builder.Property(x => x.NombreCompleto)
                .HasColumnName("nombre_completo")
                .HasMaxLength(100);

            builder.Property(x => x.JsonRespuesta)
                .HasColumnName("json_respuesta");

            // Soft delete filter
            builder.HasQueryFilter(x => x.DeletedAt == null);

            // Ignorar domain events y computed properties
            builder.Ignore(x => x.DomainEvents);
            builder.Ignore(x => x.IsDeleted);
        }
    }
}
