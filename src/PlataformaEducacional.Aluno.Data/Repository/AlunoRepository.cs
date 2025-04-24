using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Aluno.Domain;
using PlataformaEducacional.Core.Data;

namespace PlataformaEducacional.Aluno.Data
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly AlunoContext _context;

        public AlunoRepository(AlunoContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<PlataformaEducacional.Aluno.Domain.Aluno>> ObterTodos()
        {
            return await _context.Alunos.AsNoTracking().ToListAsync();
        }

        public async Task<PlataformaEducacional.Aluno.Domain.Aluno> ObterPorId(Guid id)
        {
            return await _context.Alunos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Adicionar(PlataformaEducacional.Aluno.Domain.Aluno produto)
        {
            _context.Alunos.Add(produto);
        }

        public void Atualizar(PlataformaEducacional.Aluno.Domain.Aluno produto)
        {
            _context.Alunos.Update(produto);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
