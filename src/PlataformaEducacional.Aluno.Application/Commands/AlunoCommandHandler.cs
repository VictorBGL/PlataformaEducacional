using MediatR;
using PlataformaEducacional.Aluno.AntiCorruption;
using PlataformaEducacional.Aluno.Domain;
using PlataformaEducacional.Core.Communication;
using PlataformaEducacional.Core.DomainObjects;
using PlataformaEducacional.Core.Enums;
using PlataformaEducacional.Core.Messages;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;

namespace PlataformaEducacional.Aluno.Application.Commands
{
    public class AlunoCommandHandler :
        IRequestHandler<CadastrarAlunoCommand, bool>,
        IRequestHandler<RealizarMatriculaCommand, bool>,
        IRequestHandler<FinalizarAulaCommand, bool>,
        IRequestHandler<EmitirCertificadoCommand, bool>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IAlunoRepository _alunoRepository;
        private readonly IConteudoService _conteudoService;

        public AlunoCommandHandler(IMediatorHandler mediatorHandler, IAlunoRepository alunoRepository, IConteudoService conteudoService)
        {
            _mediatorHandler = mediatorHandler;
            _alunoRepository = alunoRepository;
            _conteudoService = conteudoService;
        }

        public async Task<bool> Handle(CadastrarAlunoCommand request, CancellationToken cancellationToken)
        {
            if (!ValidarComando(request))
                return false;

            var aluno = new Domain.Aluno(request.Id, request.Nome, request.Email, request.DataNascimento);

            _alunoRepository.Adicionar(aluno);
            await _alunoRepository.UnitOfWork.Commit();

            return true;
        }

        public async Task<bool> Handle(RealizarMatriculaCommand request, CancellationToken cancellationToken)
        {
            if (!ValidarComando(request))
                return false;

            var aluno = await _alunoRepository.ObterPorId(request.AlunoId);

            if (aluno == null)
                throw new DomainException("Aluno não encontrado.");

            if (aluno.Matriculas != null && aluno.Matriculas.Any(p => p.CursoId == request.CursoId))
                throw new DomainException("Existe matrícula realizada para este curso.");

            var matricula = new Matricula(request.CursoId, request.AlunoId);

            aluno.AdicionarMatricula(matricula);

            _alunoRepository.Atualizar(aluno);
            await _alunoRepository.UnitOfWork.Commit();

            return true;
        }

        public async Task<bool> Handle(FinalizarAulaCommand request, CancellationToken cancellationToken)
        {
            if (!ValidarComando(request))
                return false;

            var aluno = await _alunoRepository.ObterPorId(request.AlunoId);

            if (aluno == null)
                throw new DomainException("Aluno não encontrado.");

            if (aluno.Matriculas == null || !aluno.Matriculas.Any(p => p.CursoId == request.CursoId))
                throw new DomainException("Matrícula não encontrada para este curso.");

            if (aluno.Matriculas.Any(p => p.CursoId == request.CursoId && p.Status == StatusMatriculaEnum.PENDENTE))
                throw new DomainException("Não é possível concluir a aula sem efetuar o pagamento.");

            if (aluno.Matriculas.Any(p => p.CursoId == request.CursoId && p.Status == StatusMatriculaEnum.CONCLUIDO))
                throw new DomainException("Não é possível concluir a aula, pois o curso já foi concluído");

            var matricula = await _alunoRepository.BuscarMatricula(aluno.Id, request.CursoId);

            var aulaFinalizada = new AulaFinalizada(request.AulaId);

            matricula.AdicionarAulaFinalizada(aulaFinalizada);

            var totalAulas = await _conteudoService.BuscarQuantidadeAulasCurso(request.CursoId);
            var percentualProgresso = (matricula.Historico.TotalAulasFinalizadas * 100) / totalAulas;
            matricula.Historico.AtualizarProgresso(percentualProgresso);

            _alunoRepository.AtualizarMatricula(matricula);
            await _alunoRepository.UnitOfWork.Commit();

            return true;
        }

        public async Task<bool> Handle(EmitirCertificadoCommand request, CancellationToken cancellationToken)
        {
            var aluno = await _alunoRepository.ObterPorId(request.AlunoId);

            if (aluno == null)
                throw new DomainException("Aluno não encontrado.");

            if (aluno.Matriculas == null || !aluno.Matriculas.Any(p => p.CursoId == request.CursoId))
                throw new DomainException("Matrícula não encontrada para este curso.");

            var matricula = await _alunoRepository.BuscarMatricula(aluno.Id, request.CursoId);

            if (matricula.Historico.PercentualProgresso >= 100)
            {
                var nomeCurso = await _conteudoService.BuscarNomeCurso(request.CursoId);
                var certificado = new Certificado(nomeCurso);

                aluno.AdicionarCertificado(certificado);
                matricula.AtualizarStatus(StatusMatriculaEnum.CONCLUIDO);
            }
            else
            {
                throw new DomainException($"O aluno não finalizou todas as aulas do curso, progresso atual {matricula.Historico.PercentualProgresso}%");
            }

            _alunoRepository.Atualizar(aluno);
            _alunoRepository.AtualizarMatricula(matricula);
            await _alunoRepository.UnitOfWork.Commit();

            return true;
        }

        private bool ValidarComando(Command message)
        {
            if (message.EhValido()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}
