using AutoMapper;
using PlataformaEducacional.Conteudo.Application.Models;
using PlataformaEducacional.Conteudo.Domain;

namespace PlataformaEducacional.Conteudo.Application.AutoMapper
{
    public class ConteudoMapperConfig : Profile
    {
        public ConteudoMapperConfig()
        {
            CreateMap<Curso, CursoResponseModel>();
            //CreateMap<CursoViewModel, Curso>();

            CreateMap<Aula, AulaResponseModel>();
            //CreateMap<AulaViewModel, Aula>().ForMember(p => p.Id, opt => opt.Ignore());

            CreateMap<ConteudoProgramaticoModel, ConteudoProgramatico>()
                .ReverseMap();
        }
    }
}
