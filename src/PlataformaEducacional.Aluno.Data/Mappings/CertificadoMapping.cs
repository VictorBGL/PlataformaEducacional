using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Aluno.Domain;

namespace PlataformaEducacional.Aluno.Data
{
    public class CertificadoMapping : IEntityTypeConfiguration<Certificado>
    {
        public void Configure(EntityTypeBuilder<Certificado> builder)
        {
            builder.HasKey(c => c.Id);

            builder.ToTable("Certificados");
        }
    }
}
