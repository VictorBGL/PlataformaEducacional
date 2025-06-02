using AutoMapper;
using PlataformaEducacional.Conteudo.Application.Interfaces;
using PlataformaEducacional.Conteudo.Application.Models;
using PlataformaEducacional.Conteudo.Domain;
using PlataformaEducacional.Conteudo.Domain.Interfaces;
using PlataformaEducacional.Conteudo.Domain.Services;
using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.Conteudo.Application.Services
{
    public class ConteudoAppService : IConteudoAppService
    {
        private readonly ICursoService _cursoService;
        private readonly IMapper _mapper;

        public ConteudoAppService(ICursoService cursoService, IMapper mapper)
        {
            _cursoService = cursoService;
            _mapper = mapper;
        }

        public async Task<List<CursoResponseModel>> FiltrarCursos(bool? ativo, string? nome)
        {
            return _mapper.Map<List<CursoResponseModel>>(await _cursoService.FiltrarCursos(ativo, nome));
        }

        public async Task<CursoResponseModel> BuscarCurso(Guid id)
        {
            return _mapper.Map<CursoResponseModel>(await _cursoService.BuscarCurso(id));
        }

        public async Task AdicionarCurso(CursoModel model)
        {
            await _cursoService.AdicionarCurso(_mapper.Map<Curso>(model));
        }

        public async Task AlterarStatusCurso(Guid id, bool ativo)
        {
            await _cursoService.AlterarStatusCurso(id, ativo);
        }

        public async Task AtualizarCurso(Guid id, CursoModel model)
        {
            await _cursoService.AtualizarCurso(id, _mapper.Map<Curso>(model));
        }

        public async Task<CursoResponseModel> AdicionarAula(Guid cursoId, AulaModel model)
        {
            if (!(await _cursoService.AdicionarAula(cursoId, _mapper.Map<Aula>(model))))
                throw new DomainException("Falha ao adicionar aula");

            return _mapper.Map<CursoResponseModel>(await _cursoService.BuscarCurso(cursoId));
        }

        public async Task<CursoResponseModel> AlterarAula(Guid cursoId, Guid aulaId, AulaModel model)
        {
            if (!(await _cursoService.AlterarAula(cursoId, aulaId, _mapper.Map<Aula>(model))))
                throw new DomainException("Falha ao alterar aula");

            return _mapper.Map<CursoResponseModel>(await _cursoService.BuscarCurso(cursoId));
        }

        public async Task<CursoResponseModel> RemoverAula(Guid cursoId, Guid aulaId)
        {
            if (!(await _cursoService.RemoverAula(cursoId, aulaId)))
                throw new DomainException("Falha ao remover aula");

            return _mapper.Map<CursoResponseModel>(await _cursoService.BuscarCurso(cursoId));
        }
    }
}
