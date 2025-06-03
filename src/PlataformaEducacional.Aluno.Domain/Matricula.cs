using PlataformaEducacional.Core.DomainObjects;
using PlataformaEducacional.Core.Enums;

namespace PlataformaEducacional.Aluno.Domain
{
    public class Matricula : EntityBase
    {
        public Matricula()
        {

        }

        public Matricula(Guid alunoId, Guid cursoId)
        {
            AlunoId = alunoId;
            CursoId = cursoId;
            Data = DateTime.Now;
            Historico = new HistoricoAprendizado();
            Status = StatusMatriculaEnum.PENDENTE;

            Validar();
        }

        public Guid AlunoId { get; private set; }
        public Guid CursoId { get; private set; }
        public DateTime Data { get; private set; }
        public StatusMatriculaEnum Status { get; private set; }
        public HistoricoAprendizado Historico { get; private set; }
        public ICollection<AulaFinalizada> AulasFinalizadas { get; private set; }
        public virtual Aluno Aluno { get; private set; }


        public void Validar()
        {
            Validacoes.ValidarSeNulo(AlunoId, "O campo AlunoId da matricula não pode estar vazio");
            Validacoes.ValidarSeNulo(CursoId, "O campo CursoId da matricula não pode estar vazio");
            Validacoes.ValidarSeNulo(Data, "O campo Data da matricula não pode estar vazio");
        }

        public void AdicionarAulaFinalizada(AulaFinalizada aulaFinalizada)
        {
            if (AulasFinalizadas == null)
                AulasFinalizadas = new List<AulaFinalizada>();

            AulasFinalizadas.Add(aulaFinalizada);

            Historico.AdicionarQuantidadeAulaFinalizada();
        }

        public void AtualizarStatus(StatusMatriculaEnum status)
        {
            Status = status;
        }
    }
}
