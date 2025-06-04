namespace PlataformaEducacional.Core.Messages.CommonMessages.IntegrationEvents
{
    public class PagamentoMatriculaEvent : IntegrationEvent
    {
        public PagamentoMatriculaEvent(Guid cursoId, Guid alunoId, string status)
        {
            CursoId = cursoId;
            AlunoId = alunoId;
            Status = status;
        }

        public Guid CursoId { get; set; }
        public Guid AlunoId { get; set; }
        public string Status { get; set; }
    }
}
