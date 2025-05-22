namespace PlataformaEducacional.Api.Models
{
    public class LoginResponseModel
    {
        public bool Authenticated { get; set; }
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UsuarioTokenResponseModel UsuarioToken { get; set; }
    }

    public class UsuarioTokenResponseModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public IEnumerable<UsuarioClaimResponseModel> Claims { get; set; }
    }

    public class UsuarioClaimResponseModel
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
