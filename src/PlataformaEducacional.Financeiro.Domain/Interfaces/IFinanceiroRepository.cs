using PlataformaEducacional.Core.Data;

namespace PlataformaEducacional.Financeiro.Domain
{
    public interface IFinanceiroRepository : IRepository<Pagamento>
    {
        void AdicionarPagamento(Pagamento pagamento);
    }
}
