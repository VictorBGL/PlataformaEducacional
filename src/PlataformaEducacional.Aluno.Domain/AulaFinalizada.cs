using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.Aluno.Domain
{
    public class AulaFinalizada : EntityBase
    {
        public AulaFinalizada(Guid aulaId)
        {
            AulaId = aulaId;
            Data = DateTime.Now;
        }

        public Guid MatriculaId { get; private set; }
        public Guid AulaId { get; private set; }
        public DateTime Data { get; private set; }
        public Matricula Matricula { get; private set; }

    }
}
