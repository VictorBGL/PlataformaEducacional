using MediatR;
using Moq;
using PlataformaEducacional.Aluno.Application.Events;
using PlataformaEducacional.Aluno.Domain;
using PlataformaEducacional.Conteudo.Domain;
using PlataformaEducacional.Core.Enums;
using PlataformaEducacional.Core.Messages.CommonMessages.IntegrationEvents;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;

namespace PlataformaEducacional.Aluno.Tests
{
    public class AlunoIntegrationEventTests
    {
        private Mock<IAlunoRepository> _alunoRepositoryMock;
        private Mock<INotificationHandler<DomainNotification>> _notificationHandlerMock;
        private AlunoIntegrationEventHandler _handler;

        [SetUp]
        public void Setup()
        {
            _alunoRepositoryMock = new Mock<IAlunoRepository>();
            _notificationHandlerMock = new Mock<INotificationHandler<DomainNotification>>();
            _handler = new AlunoIntegrationEventHandler(_notificationHandlerMock.Object, _alunoRepositoryMock.Object);
        }

        [Test]
        public async Task Deve_Atualizar_Status_Se_Pagamento_Sucesso()
        {
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var evento = new PagamentoMatriculaEvent(cursoId, alunoId, nameof(StatusPagamentoEnum.CONFIRMADO));

            var aluno = new Domain.Aluno(alunoId, "Victor Lino", "victor@teste.com", DateTime.Now.AddYears(-20));
            var matricula = new Matricula(alunoId, cursoId);
            matricula.AtualizarStatus(StatusMatriculaEnum.PENDENTE);
            aluno.AdicionarMatricula(matricula);

            _alunoRepositoryMock.Setup(x => x.ObterPorId(alunoId)).ReturnsAsync(aluno);
            _alunoRepositoryMock.Setup(x => x.BuscarMatricula(alunoId, cursoId)).ReturnsAsync(matricula);
            _alunoRepositoryMock.Setup(x => x.UnitOfWork.Commit()).ReturnsAsync(true);

            await _handler.Handle(evento, CancellationToken.None);

            _alunoRepositoryMock.Verify(x => x.AtualizarMatricula(It.IsAny<Matricula>()), Times.Once);
        }

        [Test]
        public async Task Deve_Notificar_Se_Pagamento_Falhar()
        {
            var evento = new PagamentoMatriculaEvent(Guid.NewGuid(), Guid.NewGuid(), nameof(StatusPagamentoEnum.REJEITADO));

            await _handler.Handle(evento, CancellationToken.None);

            _notificationHandlerMock.Verify(x =>
                x.Handle(It.Is<DomainNotification>(n => n.Value.Contains("Falha ao realizar pagamento")),
                         It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Deve_Notificar_Se_Aluno_Ja_Teve_Curso_Concluido()
        {
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var evento = new PagamentoMatriculaEvent(cursoId, alunoId, nameof(StatusPagamentoEnum.CONFIRMADO));

            var aluno = new Domain.Aluno(alunoId, "Victor Lino", "victor@teste.com", DateTime.Now.AddYears(-20));
            var matricula = new Matricula(alunoId, cursoId);
            matricula.AtualizarStatus(StatusMatriculaEnum.CONCLUIDO);
            aluno.AdicionarMatricula(matricula);

            _alunoRepositoryMock.Setup(x => x.ObterPorId(alunoId)).ReturnsAsync(aluno);
            _alunoRepositoryMock.Setup(x => x.BuscarMatricula(alunoId, cursoId)).ReturnsAsync(matricula);

            await _handler.Handle(evento, CancellationToken.None);

            _notificationHandlerMock.Verify(x =>
                x.Handle(It.Is<DomainNotification>(n => n.Value.Contains("curso já concluído")),
                         It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
