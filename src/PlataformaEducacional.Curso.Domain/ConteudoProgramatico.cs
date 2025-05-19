using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.Conteudo.Domain
{
    public class ConteudoProgramatico
    {
        public string Descricao { get; private set; }
        public string MaterialComplementarUrl { get; private set; }
        public int CargaHoraria { get; private set; }

        public ConteudoProgramatico(string descricao, string materialComplementarUrl)
        {
            Validacoes.ValidarSeVazio(descricao, "O campo Descrição não pode estar vazio");

            Descricao = descricao;
            MaterialComplementarUrl = materialComplementarUrl;
        }
    }
}
