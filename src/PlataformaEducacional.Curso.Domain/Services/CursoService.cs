using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Conteudo.Domain.Interfaces;
using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.Conteudo.Domain.Services
{
    public class CursoService : ICursoService
    {
        private readonly ICursoRepository _cursoRepository;

        public CursoService(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<List<Curso>> FiltrarCursos(bool? ativo, string? nome)
        {
            var cursos = await _cursoRepository.ObterTodos();

            if (ativo != null)
                cursos = cursos.Where(p => p.Ativo == ativo);

            if (!string.IsNullOrEmpty(nome))
                cursos = cursos.Where(p => p.Nome.ToUpper().Contains(nome.ToUpper()));

            return await cursos.ToListAsync();
        }

        public async Task<Curso> BuscarCurso(Guid id)
        {
            return await _cursoRepository.ObterPorId(id);
        }

        public async Task AdicionarCurso(Curso curso)
        {
            curso.Validar();
            _cursoRepository.Adicionar(curso);

            await _cursoRepository.UnitOfWork.Commit();
        }

        public async Task AlterarStatusCurso(Guid id, bool ativo)
        {
            var curso = await _cursoRepository.ObterPorId(id);
            if (curso == null)
                throw new DomainException("Curso não encontrado");

            curso.AlterarStatus(ativo);

            _cursoRepository.Atualizar(curso);

            await _cursoRepository.UnitOfWork.Commit();
        }

        public async Task AtualizarCurso(Guid id, Curso curso)
        {
            var cursoDb = await _cursoRepository.ObterPorId(id);

            if (cursoDb == null)
                throw new DomainException("Curso não encontrado");

            curso.Validar();
            cursoDb.Atualizar(curso);

            _cursoRepository.Atualizar(cursoDb);

            await _cursoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> AdicionarAula(Guid cursoId, Aula aula)
        {
            var curso = await _cursoRepository.ObterPorId(cursoId);
            if (curso == null)
                throw new DomainException("Curso não encontrado");

            curso.AdicionarAula(aula);

            _cursoRepository.Atualizar(curso);

            return await _cursoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> AtualizarAula(Guid cursoId, Guid aulaId, Aula aula)
        {
            var curso = await _cursoRepository.ObterPorId(cursoId);
            if (curso == null)
                throw new DomainException("Curso não encontrado");

            curso.AtualizarAula(aulaId, aula);

            _cursoRepository.Atualizar(curso);

            return await _cursoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> RemoverAula(Guid cursoId, Guid aulaId)
        {
            var curso = await _cursoRepository.ObterPorId(cursoId);
            if (curso == null)
                throw new DomainException("Curso não encontrado");

            curso.RemoverAula(aulaId);

            _cursoRepository.Atualizar(curso);

            return await _cursoRepository.UnitOfWork.Commit();
        }
    }
}
