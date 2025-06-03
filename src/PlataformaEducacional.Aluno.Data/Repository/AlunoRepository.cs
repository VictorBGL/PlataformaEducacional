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

        public async Task<IQueryable<PlataformaEducacional.Aluno.Domain.Aluno>> ObterTodos()
        {
            var alunos = _context.Alunos
                                    .Include(p => p.Matriculas)
                                        .ThenInclude(m => m.AulasFinalizadas)
                                    .Include(p => p.Certificados);

            return await Task.FromResult(alunos);
        }

        public async Task<PlataformaEducacional.Aluno.Domain.Aluno> ObterPorId(Guid id)
        {
            return await _context.Alunos
                                    .Include(p => p.Matriculas)
                                        .ThenInclude(m => m.AulasFinalizadas)
                                    .Include(p => p.Certificados)
                                    .FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Adicionar(PlataformaEducacional.Aluno.Domain.Aluno aluno)
        {
            _context.Alunos.Add(aluno);
        }

        public void Atualizar(PlataformaEducacional.Aluno.Domain.Aluno aluno)
        {
            _context.Alunos.Update(aluno);
        }

        public async Task<Matricula> BuscarMatricula(Guid alunoId, Guid cursoId)
        {
            return await _context.Matriculas
                                    .Include(m => m.AulasFinalizadas)
                                    .FirstOrDefaultAsync(p => p.AlunoId == alunoId && p.CursoId == cursoId);
        }

        public void AtualizarMatricula(Matricula matricula)
        {
            _context.Matriculas.Update(matricula);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
