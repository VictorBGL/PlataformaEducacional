using PlataformaEducacional.Aluno.Domain;

namespace PlataformaEducacional.Aluno.Application.Queries
{
    public class AlunoQueries : IAlunoQueries
    {
        private readonly IAlunoRepository _alunoRepository;

        public AlunoQueries(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<IQueryable<Domain.Aluno>> BuscarAlunos()
        {
            return await _alunoRepository.ObterTodos();
        }

        public async Task<Domain.Aluno> BuscarAluno(Guid alunoId)
        {
            return await _alunoRepository.ObterPorId(alunoId);
        }
    }
}
