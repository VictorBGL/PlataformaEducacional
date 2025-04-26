using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Conteudo.Domain;

namespace PlataformaEducacional.Conteudo.Data
{
    public class AulaMapping : IEntityTypeConfiguration<Aula>
    {
        public void Configure(EntityTypeBuilder<Aula> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Curso)
                .WithMany(c => c.Aulas)
                .HasForeignKey(c => c.CursoId);

            builder.ToTable("Aulas");
        }
    }
}
