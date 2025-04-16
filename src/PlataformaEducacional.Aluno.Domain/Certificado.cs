using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.Aluno.Domain
{
    public class Certificado : EntityBase
    {
        public Certificado(Guid cursoId, DateTime dataEmissao)
        {
            CursoId = cursoId;
            DataEmissao = dataEmissao;
        }


        public Guid CursoId { get; private set; }
        public DateTime DataEmissao { get; private set; }


        public void Validar()
        {
            Validacoes.ValidarSeNulo(DataEmissao, "O campo Data de emissão do certificado não pode estar vazio");
        }
    }
}
