using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Aluno.Domain;

namespace PlataformaEducacional.Aluno.Data
{
    public class MatriculaMapping : IEntityTypeConfiguration<Matricula>
    {
        public void Configure(EntityTypeBuilder<Matricula> builder)
        {
            builder.HasKey(c => c.Id);

            builder.ToTable("Matriculas");
        }
    }
}
