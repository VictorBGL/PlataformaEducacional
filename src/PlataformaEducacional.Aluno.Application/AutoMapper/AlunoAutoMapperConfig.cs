using AutoMapper;
using PlataformaEducacional.Aluno.Application.Models;
using PlataformaEducacional.Aluno.Domain;

namespace PlataformaEducacional.Aluno.Application.AutoMapper
{
    public class AlunoMapperConfig : Profile
    {
        public AlunoMapperConfig()
        {
            CreateMap<Domain.Aluno, AlunoResponseModel>();

            CreateMap<Certificado, CertificadoResponseModel>();

            CreateMap<Matricula, MatriculaResponseModel>()
                .ForMember(d => d.StatusMatricula, o => o.MapFrom(s => s.Status.ToString()));

            CreateMap<HistoricoAprendizadoModel, HistoricoAprendizado>()
                .ReverseMap();
        }
    }
}
