using PlataformaEducacional.Core.Data;

namespace PlataformaEducacional.Financeiro.Domain
{
    public interface IFinanceiroRepository : IRepository<Pagamento>
    {
        void Adicionar(Pagamento pagamento);
    }
}
