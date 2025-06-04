using FluentValidation;
using PlataformaEducacional.Core.Messages;

namespace PlataformaEducacional.Financeiro.Apllication.Commands
{
    public class PagamentoMatriculaCommand : Command
    {
        public PagamentoMatriculaCommand(Guid alunoId, Guid cursoId, string nomeTitular, string numeroCartao, string cvvCartao, DateTime validade, decimal valorCurso)
        {
            AlunoId = alunoId;
            CursoId = cursoId;
            NomeTitular = nomeTitular;
            NumeroCartao = numeroCartao;
            CvvCartao = cvvCartao;
            Validade = validade;
            ValorCurso = valorCurso;
        }

        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }
        public string NomeTitular { get; set; }
        public string NumeroCartao { get; set; }
        public string CvvCartao { get; set; }
        public DateTime Validade { get; set; }
        public decimal ValorCurso { get; set; }

        public override bool EhValido()
        {
            ValidationResult = new PagamentoMatriculaValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }


    public class PagamentoMatriculaValidation : AbstractValidator<PagamentoMatriculaCommand>
    {
        public PagamentoMatriculaValidation()
        {
            RuleFor(c => c.AlunoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do aluno inválido");

            RuleFor(c => c.CursoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Curso não informado.");

            RuleFor(c => c.NomeTitular)
                .NotEqual(string.Empty)
                .WithMessage("Nome do titular inválido.");

            RuleFor(c => c.NumeroCartao)
              .NotEqual(string.Empty)
              .WithMessage("Numero do cartão inválido")
              .MinimumLength(16)
              .WithMessage("Numero do cartão inválido");

            RuleFor(c => c.Validade)
              .NotNull()
              .WithMessage("Validade não informada.")
              .GreaterThan(DateTime.Now)
              .WithMessage("Validade do cartão inválida");

            RuleFor(c => c.CvvCartao)
              .NotEqual(string.Empty)
              .WithMessage("Ccv inválido")
              .MinimumLength(3)
              .WithMessage("Ccv inválido");
        }
    }
}
