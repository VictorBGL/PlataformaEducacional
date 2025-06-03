namespace PlataformaEducacional.Aluno.Domain
{
    public class HistoricoAprendizado
    {
        public HistoricoAprendizado()
        {
            TotalAulasFinalizadas = 0;
            PercentualProgresso = 0;
        }

        public double PercentualProgresso { get; private set; }
        public int TotalAulasFinalizadas { get; private set; }


        public void AdicionarQuantidadeAulaFinalizada()
        {
            TotalAulasFinalizadas += 1;
        }

        public void AtualizarProgresso(double percentual)
        {
            PercentualProgresso = percentual;
        }
    }
}
