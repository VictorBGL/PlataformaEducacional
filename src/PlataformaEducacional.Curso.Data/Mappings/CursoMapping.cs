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

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.OwnsOne(c => c.Conteudo, cm =>
            {
                cm.Property(c => c.Descricao)
                    .HasColumnName("Descricao")
                    .HasColumnType("varchar(500)");

                cm.Property(c => c.MaterialComplementarUrl)
                    .HasColumnName("MaterialComplementarUrl")
                    .HasColumnType("varchar(250)");
            });

            builder.ToTable("Cursos");
        }
    }
}
