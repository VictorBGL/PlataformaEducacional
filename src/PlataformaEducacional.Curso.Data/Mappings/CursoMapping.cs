using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Conteudo.Domain;

namespace PlataformaEducacional.Conteudo.Data
{
    public class CursoMapping : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.HasKey(c => c.Id);

            builder.OwnsOne(c => c.Conteudo, cm =>
            {
                cm.Property(c => c.Descricao);
                cm.Property(c => c.MaterialComplementarUrl).IsRequired(false);
            });

            builder.ToTable("Curso");
        }
    }
}
