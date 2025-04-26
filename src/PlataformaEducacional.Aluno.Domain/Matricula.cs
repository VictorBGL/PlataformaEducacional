using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.Aluno.Domain
{
    public class Matricula : EntityBase
    {
        public Matricula(Guid alunoId, Guid cursoId, DateTime dataMatricula)
        {
            AlunoId = alunoId;
            CursoId = cursoId;
            Data = dataMatricula;
            Concluido = false;

            Validar();
        }


        public Guid AlunoId { get; private set; }
        public Guid CursoId { get; private set; }
        public DateTime Data { get; private set; }
        public bool Concluido { get; private set; }
        public virtual Aluno Aluno { get; private set; }


        public void Validar()
        {
            Validacoes.ValidarSeNulo(Data, "O campo Data da matricula não pode estar vazio");
        }

        public void ConcluirCurso()
        {
            Concluido = true;
        }
    }
}
