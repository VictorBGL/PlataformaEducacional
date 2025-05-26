namespace PlataformaEducacional.Api.Entities
{
    public class Administrador
    {
        protected Administrador(){}

        public Administrador(Guid id, string nome, string email, bool ativo)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Ativo = ativo;
        }

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }
    }
}
