using FluentValidation;
using PlataformaEducacional.Core.Messages;

namespace PlataformaEducacional.Aluno.Application.Commands
{
    public class FinalizarAulaCommand : Command
    {
        public FinalizarAulaCommand(Guid alunoId, Guid cursoId, Guid aulaId)
        {
            AlunoId = alunoId;
            AulaId = aulaId;
            CursoId = cursoId;
        }

        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }
        public Guid AulaId { get; set; }

        public override bool EhValido()
        {
            ValidationResult = new FinalizarAulaValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class FinalizarAulaValidation : AbstractValidator<FinalizarAulaCommand>
    {
        public FinalizarAulaValidation()
        {
            RuleFor(c => c.AulaId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id da aula inválido");

            RuleFor(c => c.AlunoId)
                   .NotEqual(Guid.Empty)
                   .WithMessage("Id do aluno inválido");

            RuleFor(c => c.CursoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do curso inválido");
        }
    }
}
