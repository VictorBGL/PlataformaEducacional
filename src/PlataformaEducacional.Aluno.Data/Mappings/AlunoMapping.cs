using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Aluno.Domain;

namespace PlataformaEducacional.Aluno.Data
{
    public class AlunoMapping : IEntityTypeConfiguration<PlataformaEducacional.Aluno.Domain.Aluno>
    {
        public void Configure(EntityTypeBuilder<PlataformaEducacional.Aluno.Domain.Aluno> builder)
        {
            builder.ToTable("Alunos");

            builder.HasKey(c => c.Id);

            builder.HasMany(c => c.Certificados)
                .WithOne(c => c.Aluno)
                .HasForeignKey(c => c.AlunoId);

            builder.HasMany(c => c.Matriculas)
                .WithOne(c => c.Aluno)
                .HasForeignKey(c => c.AlunoId);
        }
    }
}
