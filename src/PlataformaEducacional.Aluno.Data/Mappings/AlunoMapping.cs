using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Aluno.Domain;

namespace PlataformaEducacional.Aluno.Data
{
    public class AlunoMapping : IEntityTypeConfiguration<PlataformaEducacional.Aluno.Domain.Aluno>
    {
        public void Configure(EntityTypeBuilder<PlataformaEducacional.Aluno.Domain.Aluno> builder)
        {
            builder.HasKey(c => c.Id);

            builder.OwnsOne(c => c.Historico, cm =>
            {
                //cm.Property(c => c.Descricao)
                //    .HasColumnName("Descricao")
                //    .HasColumnType("varchar(500)");

                //cm.Property(c => c.MaterialComplementarUrl)
                //    .HasColumnName("MaterialComplementarUrl")
                //    .HasColumnType("varchar(250)");
            });

            builder.HasMany(c => c.Certificados)
                .WithOne(c => c.Aluno)
                .HasForeignKey(c => c.AlunoId);

            builder.HasMany(c => c.Matriculas)
                .WithOne(c => c.Aluno)
                .HasForeignKey(c => c.AlunoId);

            builder.ToTable("Alunos");
        }
    }
}
