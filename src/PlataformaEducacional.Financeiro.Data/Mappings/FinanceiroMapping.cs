using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Financeiro.Domain;

namespace PlataformaEducacional.Financeiro.Data
{
    public class FinanceiroMapping : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.HasKey(x => x.Id);

            builder.OwnsOne(c => c.DadosCartao, cm =>
            {
                cm.Property(c => c.NumeroMascarado);
                cm.Property(c => c.NomeTitular);
                cm.Property(c => c.CvvCartao);
                cm.Property(c => c.Validade);
            });

            builder.OwnsOne(c => c.Status, cm =>
            {
                cm.Property(c => c.MotivoRejeicao);
                cm.Property(c => c.Status);
            });

            builder.ToTable("Pagamento");
        }
    }
}
