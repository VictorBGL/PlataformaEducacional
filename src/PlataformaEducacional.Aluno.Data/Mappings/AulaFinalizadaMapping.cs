using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Aluno.Domain;

namespace PlataformaEducacional.Aluno.Data.Mappings
{
    public class AulaFinalizadaMapping : IEntityTypeConfiguration<AulaFinalizada>
    {
        public void Configure(EntityTypeBuilder<AulaFinalizada> builder)
        {
            builder.ToTable("AulaFinalizada");

            builder.HasKey(x => x.Id);
        }
    }
}
