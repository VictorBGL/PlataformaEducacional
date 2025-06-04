using System.ComponentModel.DataAnnotations;

namespace PlataformaEducacional.Api.Models.Auth
{
    public class NovaContaModel
    {
        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatório")]
        public string Senha { get; set; }

        [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmarSenha { get; set; }

        [Required(ErrorMessage = "O campo data nascimento é obrigatório")]
        public DateTime DataNascimento { get; set; }
    }
}
