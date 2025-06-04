using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEducacional.Aluno.Application.Commands;
using PlataformaEducacional.Aluno.Application.Models;
using PlataformaEducacional.Aluno.Application.Queries;
using PlataformaEducacional.Conteudo.Application.Models;
using PlataformaEducacional.Core.Communication;
using PlataformaEducacional.Core.Enums;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.Financeiro.Apllication.Commands;
using PlataformaEducacional.Financeiro.Apllication.Models;

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

        /// <summary>
        /// Filtrar alunos
        /// </summary>
        [Authorize(Roles = nameof(RoleUsuarioEnum.ADMINISTRADOR))]
        [ProducesResponseType(typeof(List<AlunoResponseModel>), 200)]
        [HttpGet]
        public async Task<IActionResult> FiltrarAlunos([FromBody] AlunoFiltroModel filtro)
        {
            var alunos = await _alunoQueries.FiltrarAlunos(filtro.Nome, filtro.ExisteMatriculaAtiva);
            return Ok(alunos);
        }

        /// <summary>
        /// Buscar aluno por id
        /// </summary>
        [ProducesResponseType(typeof(AlunoResponseModel), 200)]
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarAluno(Guid id)
        {
            return Ok(await _alunoQueries.BuscarAluno(id));
        }

        /// <summary>
        /// Matricular aluno em um curso
        /// </summary>
        [HttpPost("{id}/curso/{cursoId}/matricular")]
        public async Task<IActionResult> RealizarMatricula([FromRoute] Guid id, Guid cursoId)
        {
            await _mediatorHandler.EnviarComando(new RealizarMatriculaCommand(cursoId, id));

            if (OperacaoValida())
                return Ok("Matricula realizada com sucesso, e pendente de pagamento!");

            return BadRequest(ObterMensagensErro());
        }

        /// <summary>
        /// Finalizar a aula de um curso
        /// </summary>
        [HttpPost("{id}/curso/{cursoId}/aula/{aulaId}/finalizar")]
        public async Task<IActionResult> ConcluirAula(Guid id, Guid cursoId, Guid aulaId)
        {
            var command = new FinalizarAulaCommand(id, cursoId, aulaId);
            await _mediatorHandler.EnviarComando(command);

            if (OperacaoValida())
                return Ok("Aula finalizada com sucesso!");

            return BadRequest(ObterMensagensErro());
        }

        /// <summary>
        /// Emitir certificado de conclusão de curso
        /// </summary>
        [HttpPost("{id}/curso/{cursoId}/certificado")]
        public async Task<IActionResult> EmitirCertificado(Guid id, Guid cursoId)
        {
            var command = new EmitirCertificadoCommand(cursoId, id);
            await _mediatorHandler.EnviarComando(command);

            if (OperacaoValida())
                return Ok("Certifcado emitido com sucesso! Consulte seu perfil para vistualiza-lo!");

            return BadRequest(ObterMensagensErro());
        }

        /// <summary>
        /// Buscar Certificados disponiveis
        /// </summary>
        //[HttpGet("{id}/certificados")]
        //public async Task<IActionResult> BuscarCertificados(Guid id)
        //{
        //    var command = new EmitirCertificadoCommand(cursoId, id);
        //    await _mediatorHandler.EnviarComando(command);

        //    if (OperacaoValida())
        //        return Ok("Certifcado emitido com sucesso! Consulte seu perfil para vistualiza-lo!");

        //    return BadRequest(ObterMensagensErro());
        //}

        /// <summary>
        /// Realizar pagamento da matrícula em um curso
        /// </summary>
        [HttpPost("{id}/curso/{cursoId}/pagamento")]
        public async Task<IActionResult> PagamentoMatricula([FromRoute] Guid id, Guid cursoId, [FromBody] PagamentoModel model)
        {
            var command = new PagamentoMatriculaCommand(id, cursoId, model.NomeTitular, model.NumeroCartao, model.CvvCartao, model.Validade, model.ValorCurso);
            await _mediatorHandler.EnviarComando(command);

            if (OperacaoValida())
                return Ok("Pagamento efetuado com sucesso!");

            return BadRequest(ObterMensagensErro());
        }
    }
}
