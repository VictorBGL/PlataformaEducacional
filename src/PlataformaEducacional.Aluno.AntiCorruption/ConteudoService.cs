using PlataformaEducacional.Conteudo.Domain.Interfaces;

namespace PlataformaEducacional.Aluno.AntiCorruption
{
    public class ConteudoService : IConteudoService
    {
        private readonly ICursoService _cursoService;

        public ConteudoService(ICursoService cursoService)
        {
            _cursoService = cursoService;
        }

        public async Task<int> BuscarQuantidadeAulasCurso(Guid cursoId)
        {
            var curso = await _cursoService.BuscarCurso(cursoId);

            if (curso.Aulas == null)
                return 0;

            return curso.Aulas.Count();
        }

        public async Task<string> BuscarNomeCurso(Guid cursoId)
        {
            var curso = await _cursoService.BuscarCurso(cursoId);

            return curso.Nome;
        }
    }
}
