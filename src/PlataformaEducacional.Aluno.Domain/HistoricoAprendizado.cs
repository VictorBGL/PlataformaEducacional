namespace PlataformaEducacional.Aluno.Domain
{
    public class HistoricoAprendizado
    {
        public List<RegistroAula> AulasAssistidas { get; private set; }

        public void AdicionarRegistro(string tituloAula, DateTime dataVisualizacao)
        {
            AulasAssistidas.Add(new RegistroAula(tituloAula, dataVisualizacao));
        }
    }

    public class RegistroAula
    {
        public string TituloAula { get; private set; }
        public DateTime DataVisualizacao { get; private set; }

        public RegistroAula(string tituloAula, DateTime dataVisualizacao)
        {
            TituloAula = tituloAula;
            DataVisualizacao = dataVisualizacao;
        }
    }
}
