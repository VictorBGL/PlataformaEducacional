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

            builder.ToTable("Pagamento");
        }
    }
}
