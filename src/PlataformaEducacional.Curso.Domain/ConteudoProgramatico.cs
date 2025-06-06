using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.Conteudo.Domain
{
    public class ConteudoProgramatico
    {
        public string Descricao { get; private set; }
        public string? MaterialComplementarUrl { get; private set; }
        public string CargaHoraria { get; private set; }

        public ConteudoProgramatico(string descricao, string materialComplementarUrl, string cargaHoraria)
        {
            Validacoes.ValidarSeVazio(descricao, "O campo Descrição não pode estar vazio");
            Validacoes.ValidarSeVazio(cargaHoraria, "O campo Carga horária não pode estar vazio");

            Descricao = descricao;
            MaterialComplementarUrl = materialComplementarUrl;
            CargaHoraria = cargaHoraria;
        }

        public void Validar()
        {
            Validacoes.ValidarSeVazio(Descricao, "O campo Descrição não pode estar vazio");
            Validacoes.ValidarSeVazio(CargaHoraria, "O campo Carga horária não pode estar vazio");
        }
    }
}
