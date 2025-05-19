using PlataformaEducacional.Core.Data;
using PlataformaEducacional.Financeiro.Domain;

namespace PlataformaEducacional.Financeiro.Data.Repository
{
    public class FinanceiroRepository : IFinanceiroRepository
    {
        private readonly FinanceiroContext _context;

        public FinanceiroRepository(FinanceiroContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void AdicionarPagamento(Pagamento pagamento)
        {
            _context.Add(pagamento);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
