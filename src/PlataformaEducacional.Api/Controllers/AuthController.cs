using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PlataformaEducacional.Api.Extensions;
using PlataformaEducacional.Api.Models;
using PlataformaEducacional.Core.Communication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PlataformaEducacional.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppSettings _appSettings;
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;
        private readonly IMediatorHandler _mediatorHandler;

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<AppSettings> appSettings,
            IPasswordHasher<IdentityUser> passwordHasher,
            IMediatorHandler mediatorHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _appSettings = appSettings.Value;
            _passwordHasher = passwordHasher;
            _mediatorHandler = mediatorHandler;
        }

        /// <summary>
        /// Endpoint para login do usuário
        /// </summary>
        /// <response code="200">Jwt token para acesso</response>
        /// <response code="400">Conteúdo inválido</response>
        /// <response code="500">Erro interno</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(IEnumerable<LoginResponseModel>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await
                    _signInManager.PasswordSignInAsync(model.Email, model.Senha, false, false);

                if (result.Succeeded)
                {
                    var token = await GerarJwt(model.Email, string.Empty);
                    return Ok(new { sucesso = true, token = token });
                }
            }

            return BadRequest(new { sucesso = false, message = "E-mail ou senha incorretos" });
        }

        private async Task<LoginResponseModel> GerarJwt(string email, string nomeUsuario)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);

            var claims = await _userManager.GetClaimsAsync(identityUser);

            var identityClaims = await ObterClaimsUsuario(claims, identityUser, nomeUsuario);
            var encodedToken = CodificarToken(identityClaims);

            return await ObterRespostaToken(encodedToken, identityUser, nomeUsuario, claims);
        }

        private async Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims, IdentityUser identityUser,
            string usuarioNome)
        {
            var userRoles = await _userManager.GetRolesAsync(identityUser);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, identityUser.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, identityUser.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Name, usuarioNome));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(),
                ClaimValueTypes.Integer64));
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp,
                ToUnixEpochDate(DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras)).ToString(),
                ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private string CodificarToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        private async Task<LoginResponseModel> ObterRespostaToken(string encodedToken,
            IdentityUser identityUser, string nomeUsuario, IEnumerable<Claim> claims)
        {
            return new LoginResponseModel
            {
                Authenticated = true,
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalHours,
                UsuarioToken = new UsuarioTokenResponseModel
                {
                    Id = identityUser.Id,
                    Email = identityUser.Email,
                    Nome = nomeUsuario,
                    Claims = claims.Select(c => new UsuarioClaimResponseModel { Type = c.Type, Value = c.Value })
                }
            };
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);
    }
}
