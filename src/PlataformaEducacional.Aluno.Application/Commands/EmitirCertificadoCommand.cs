using FluentValidation;
using PlataformaEducacional.Core.Messages;

namespace PlataformaEducacional.Aluno.Application.Commands
{
    public class EmitirCertificadoCommand : Command
    {
        public EmitirCertificadoCommand(Guid cursoId, Guid alunoId)
        {
            CursoId = cursoId;
            AlunoId = alunoId;
        }

        public Guid CursoId { get; set; }
        public Guid AlunoId { get; set; }

        public override bool EhValido()
        {
            ValidationResult = new EmitirCertificadoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class EmitirCertificadoValidation : AbstractValidator<EmitirCertificadoCommand>
    {
        public EmitirCertificadoValidation()
        {
            RuleFor(c => c.CursoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do curso inválido");

            RuleFor(c => c.AlunoId)
                   .NotEqual(Guid.Empty)
                   .WithMessage("Id do aluno inválido");
        }
    }
}
