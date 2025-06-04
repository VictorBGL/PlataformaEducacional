using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PlataformaEducacional.Aluno.Application.Commands;
using PlataformaEducacional.Api.Data;
using PlataformaEducacional.Api.Entities;
using PlataformaEducacional.Api.Extensions;
using PlataformaEducacional.Api.Models;
using PlataformaEducacional.Api.Models.Auth;
using PlataformaEducacional.Core.Communication;
using PlataformaEducacional.Core.Enums;
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
        private readonly Context _context;
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;
        private readonly IMediatorHandler _mediatorHandler;

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<AppSettings> appSettings,
            IPasswordHasher<IdentityUser> passwordHasher,
            IMediatorHandler mediatorHandler,
            Context context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _appSettings = appSettings.Value;
            _passwordHasher = passwordHasher;
            _mediatorHandler = mediatorHandler;
            _context = context;
        }

        /// <summary>
        /// Realizar Login do usuário
        /// </summary>
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

        /// <summary>
        /// Criação de conta para administrador
        /// </summary>
        [HttpPost("cadastrar-administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CadastrarAdministrador(NovaContaModel model)
        {
            if (ModelState.IsValid)
            {
                var identityUser = new IdentityUser(model.Email);
                identityUser.PasswordHash = _passwordHasher.HashPassword(identityUser, model.Senha);
                identityUser.Email = model.Email;

                var result = await _userManager.CreateAsync(identityUser);

                if (result.Succeeded)
                {
                    await CreateRoles();
                    await _userManager.AddToRoleAsync(identityUser, nameof(RoleUsuarioEnum.ADMINISTRADOR));

                    var admininstrador = new Administrador(Guid.Parse(identityUser.Id), model.Nome, model.Email, model.DataNascimento);
                    _context.Set<Administrador>().Add(admininstrador);
                    await _context.SaveChangesAsync();

                    return Ok(new { sucesso = true });
                }

                var erros = result.Errors.Select(x => x.Description).ToArray();
                return BadRequest(new { sucesso = false, erros = erros });
            }

            return BadRequest(new { sucesso = false, erros = ModelState });
        }

        /// <summary>
        /// Criação de conta para aluno
        /// </summary>
        [HttpPost("cadastrar-aluno")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CadastrarAluno(NovaContaModel model)
        {
            if (ModelState.IsValid)
            {
                var identityUser = new IdentityUser(model.Email);
                identityUser.PasswordHash = _passwordHasher.HashPassword(identityUser, model.Senha);
                identityUser.Email = model.Email;

                var result = await _userManager.CreateAsync(identityUser);

                if (result.Succeeded)
                {
                    await CreateRoles();
                    await _userManager.AddToRoleAsync(identityUser, nameof(RoleUsuarioEnum.ALUNO));

                    var command = new CadastrarAlunoCommand(Guid.Parse(identityUser.Id), model.Nome, model.Email, model.DataNascimento);
                    await _mediatorHandler.EnviarComando(command);

                    return Ok(new { sucesso = true });
                }

                var erros = result.Errors.Select(x => x.Description).ToArray();
                return BadRequest(new { sucesso = false, erros = erros });
            }

            return BadRequest(new { sucesso = false, erros = ModelState });
        }

        private async Task CreateRoles()
        {
            string[] rolesNames =
            {
            nameof(RoleUsuarioEnum.ADMINISTRADOR),
            nameof(RoleUsuarioEnum.ALUNO),
        };

            foreach (var namesRole in rolesNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(namesRole);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(namesRole));
                }
            }
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
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, identityUser.Email ?? string.Empty));
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
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret ?? string.Empty);
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
            var resposta = new LoginResponseModel
            {
                Authenticated = true,
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalHours,
                UsuarioToken = new UsuarioTokenResponseModel
                {
                    Id = identityUser.Id,
                    Email = identityUser.Email ?? string.Empty,
                    Nome = nomeUsuario,
                    Claims = claims.Select(c => new UsuarioClaimResponseModel { Type = c.Type, Value = c.Value })
                }
            };

            return await Task.FromResult(resposta);
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);
    }
}
