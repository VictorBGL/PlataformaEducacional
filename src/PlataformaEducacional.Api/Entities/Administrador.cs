namespace PlataformaEducacional.Api.Entities
{
    public class Administrador
    {
        protected Administrador(){}

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }
    }
}
