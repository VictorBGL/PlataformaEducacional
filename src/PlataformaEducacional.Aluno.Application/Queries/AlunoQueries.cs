using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Aluno.Application.Models;
using PlataformaEducacional.Aluno.Domain;
using PlataformaEducacional.Core.Enums;

namespace PlataformaEducacional.Aluno.Application.Queries
{
    public class AlunoQueries : IAlunoQueries
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IMapper _mapper;

        public AlunoQueries(IAlunoRepository alunoRepository, IMapper mapper)
        {
            _alunoRepository = alunoRepository;
            _mapper = mapper;
        }

        public async Task<List<AlunoResponseModel>> FiltrarAlunos(string? nome, bool? existeMatriculaAtiva)
        {
            var alunos = await _alunoRepository.ObterTodos();

            if (existeMatriculaAtiva == true)
                alunos = alunos.Where(p => p.Matriculas != null && p.Matriculas.Any(x => x.Status == StatusMatriculaEnum.ATIVO));

            if (existeMatriculaAtiva == false)
                alunos = alunos.Where(p => p.Matriculas == null || !p.Matriculas.Any(x => x.Status == StatusMatriculaEnum.ATIVO));

            if (!string.IsNullOrEmpty(nome))
                alunos = alunos.Where(p => p.NomeCompleto.ToUpper().Contains(nome.ToUpper()));

            alunos = alunos.OrderByDescending(p => p.DataCadastro);

            return _mapper.Map<List<AlunoResponseModel>>(await alunos.ToListAsync());
        }

        public async Task<AlunoResponseModel> BuscarAluno(Guid alunoId)
        {
            return _mapper.Map<AlunoResponseModel>(await _alunoRepository.ObterPorId(alunoId));
        }

        public async Task<List<CertificadoResponseModel>> BuscarCertificadosAluno(Guid alunoId)
        {
            var aluno = await _alunoRepository.ObterPorId(alunoId);

            if (aluno == null || aluno.Certificados == null || !aluno.Certificados.Any())
            {
                return new List<CertificadoResponseModel>();
            }

            return _mapper.Map<List<CertificadoResponseModel>>(aluno.Certificados.ToList());
        }
    }
}
