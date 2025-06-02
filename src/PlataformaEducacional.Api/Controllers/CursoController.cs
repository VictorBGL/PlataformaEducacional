using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEducacional.Conteudo.Application.Interfaces;
using PlataformaEducacional.Conteudo.Application.Models;
using PlataformaEducacional.Core.Communication;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;

namespace PlataformaEducacional.Api.Controllers
{
    [Authorize]
    [Route("api/curso")]
    public class CursoController : BaseController
    {
        private readonly IConteudoAppService _conteudoAppService;

        public CursoController(
            IConteudoAppService conteudoAppService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            _conteudoAppService = conteudoAppService;
        }

        /// <summary>
        /// Filtrar cursos
        /// </summary>
        [ProducesResponseType(typeof(List<CursoResponseModel>), 200)]
        [HttpGet]
        public async Task<IActionResult> FiltrarCursos([FromBody] CursoFiltroModel filtro)
        {
            var cursos = await _conteudoAppService.FiltrarCursos(filtro.Ativo, filtro.Nome);

            return Ok(cursos);
        }

        /// <summary>
        /// Busca curso por id
        /// </summary>
        [ProducesResponseType(typeof(CursoResponseModel), 200)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Buscar(Guid id)
        {
            var curso = await _conteudoAppService.BuscarCurso(id);
            return Ok(curso);
        }

        /// <summary>
        /// Inserir um novo curso
        /// </summary>
        //[Authorize(Roles = nameof(PerfilUsuarioEnum.ADM))]
        [HttpPost]
        public async Task<IActionResult> Adicionar(CursoModel model)
        {
            await _conteudoAppService.AdicionarCurso(model);
            return Ok();
        }

        /// <summary>
        /// Ativa ou inativa um curso
        /// </summary>
        //[Authorize(Roles = nameof(PerfilUsuarioEnum.ADM))]
        [HttpPost("{id}/status")]
        public async Task<IActionResult> AlterarStatus([FromRoute] Guid id, [FromBody] CursoStatusModel model)
        {
            await _conteudoAppService.AlterarStatusCurso(id, model.Ativo);
            return Ok();
        }

        /// <summary>
        /// Atualizar um curso
        /// </summary>
        //[Authorize(Roles = nameof(PerfilUsuarioEnum.ADM))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar([FromRoute] Guid id, [FromBody] CursoModel model)
        {
            await _conteudoAppService.AtualizarCurso(id, model);
            return Ok();
        }
    }
}
