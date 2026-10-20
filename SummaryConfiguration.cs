using AC.Core.Entities;
using AC.Core.Modules.Summaries.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AC.Infrastructure.Data.Configurations;

// Mapeo de la entidad Summary a la tabla "summaries" (snake_case en PostgreSQL).
public class SummaryConfiguration : IEntityTypeConfiguration<Summary>
{
    public void Configure(EntityTypeBuilder<Summary> builder)
    {
        builder.ToTable("summaries");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.Content).HasColumnName("content").IsRequired();
        builder.Property(s => s.Source).HasColumnName("source")
            .HasMaxLength(CreateSummaryValidator.SourceMaxLength);
        builder.Property(s => s.NewsCount).HasColumnName("news_count");
        builder.Property(s => s.CreatedAt).HasColumnName("created_at");

        // Índice para traer rápido los más recientes.
        builder.HasIndex(s => s.CreatedAt);
    }
}
