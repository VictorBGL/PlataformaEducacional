using FluentValidation;
using PlataformaEducacional.Core.Messages;

namespace PlataformaEducacional.Aluno.Application.Commands
{
    public class CadastrarAlunoCommand : Command
    {
        public CadastrarAlunoCommand(Guid id, string nome, string email, DateTime dataNascimento)
        {
            Id = id;
            Nome = nome;
            Email = email;
            DataNascimento = dataNascimento;
        }

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }


        public override bool EhValido()
        {
            ValidationResult = new CadastrarAlunoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CadastrarAlunoValidation : AbstractValidator<CadastrarAlunoCommand>
    {
        public CadastrarAlunoValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do aluno inválido");

            RuleFor(c => c.Nome)
                .NotEqual(string.Empty)
                .WithMessage("Nome não informado.");

            RuleFor(c => c.Email)
                .EmailAddress()
                .WithMessage("E-mail inválido.");
        }
    }
}
