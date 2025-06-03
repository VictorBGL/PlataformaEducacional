using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Aluno.Domain;

namespace PlataformaEducacional.Aluno.Data
{
    public class MatriculaMapping : IEntityTypeConfiguration<Matricula>
    {
        public void Configure(EntityTypeBuilder<Matricula> builder)
        {
            builder.ToTable("Matriculas");

            builder.HasKey(c => c.Id);

            builder.Property(p => p.Status);

            builder.OwnsOne(c => c.Historico, cm =>
            {
                cm.Property(c => c.PercentualProgresso);
                cm.Property(c => c.TotalAulasFinalizadas);
            });

            builder.HasMany(c => c.AulasFinalizadas)
                .WithOne(c => c.Matricula)
                .HasForeignKey(c => c.MatriculaId);
        }
    }
}
