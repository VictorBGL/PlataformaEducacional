using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Conteudo.Domain;
using PlataformaEducacional.Core.Data;

namespace PlataformaEducacional.Conteudo.Data
{
    public class CursoRepository : ICursoRepository
    {
        private readonly ConteudoContext _context;

        public CursoRepository(ConteudoContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IQueryable<Curso>> ObterTodos()
        {
            return await Task.FromResult(_context.Cursos.AsNoTracking().Include(p => p.Aulas));
        }

        public async Task<Curso> ObterPorId(Guid id)
        {
            return await _context.Cursos
                                    .Include(p => p.Aulas)
                                    .FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Adicionar(Curso curso)
        {
            _context.Cursos.Add(curso);
        }

        public void Atualizar(Curso curso)
        {
            _context.Cursos.Update(curso);
        }

        public void Remover(Curso curso)
        {
            _context.Cursos.Remove(curso);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
