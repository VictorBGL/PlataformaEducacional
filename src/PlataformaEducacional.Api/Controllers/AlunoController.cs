using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEducacional.Aluno.Application.Commands;
using PlataformaEducacional.Aluno.Application.Queries;
using PlataformaEducacional.Core.Communication;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;

namespace PlataformaEducacional.Api.Controllers
{
    [Authorize]
    [Route("api/aluno")]
    public class AlunoController : BaseController
    {
        private readonly IAlunoQueries _alunoQueries;

        public AlunoController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler,
            IAlunoQueries alunoQueries) : base(notifications, mediatorHandler)
        {
            _alunoQueries = alunoQueries;
        }

        [HttpGet]
        public IActionResult BuscarAlunos()
        {
            return Ok(_alunoQueries.BuscarAlunos());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarAluno(Guid id)
        {
            return Ok(await _alunoQueries.BuscarAluno(id));
        }

        [HttpPost("{id}/curso/{cursoId}/matricular")]
        public async Task<IActionResult> RealizarMatricula([FromRoute] Guid id, Guid cursoId)
        {
            await _mediatorHandler.EnviarComando(new RealizarMatriculaCommand(cursoId, id));

            if (OperacaoValida())
                return Ok("Matricula realizada com sucesso!");

            return BadRequest(ObterMensagensErro());
        }

        [HttpPost("{id}/curso/{cursoId}/aula/{aulaId}/finalizar")]
        public async Task<IActionResult> ConcluirAula(Guid id, Guid cursoId, Guid aulaId)
        {
            var command = new FinalizarAulaCommand(id, cursoId, aulaId);
            await _mediatorHandler.EnviarComando(command);

            if (OperacaoValida())
                return Ok("Aula concluída com sucesso!");

            return BadRequest(ObterMensagensErro());
        }

        [HttpPost("{id}/curso/{cursoId}/certificado")]
        public async Task<IActionResult> EmitirCertificado(Guid id, Guid cursoId)
        {
            var command = new EmitirCertificadoCommand(cursoId, id);
            await _mediatorHandler.EnviarComando(command);

            if (OperacaoValida())
                return Ok("Certifcado emitido com sucesso! Consulte seu perfil para vistualiza-lo!");

            return BadRequest(ObterMensagensErro());
        }
    }
}
