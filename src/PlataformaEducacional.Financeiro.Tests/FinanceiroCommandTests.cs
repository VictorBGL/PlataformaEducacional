using Moq;
using PlataformaEducacional.Core.Communication;
using PlataformaEducacional.Core.Enums;
using PlataformaEducacional.Core.Messages.CommonMessages.IntegrationEvents;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.Financeiro.Apllication.Commands;
using PlataformaEducacional.Financeiro.Domain;

namespace PlataformaEducacional.Financeiro.Tests
{
    public class FinanceiroCommandTests
    {
        private Mock<IMediatorHandler> _mediatorHandlerMock;
        private Mock<IFinanceiroRepository> _financeiroRepositoryMock;
        private FinanceiroCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
            _financeiroRepositoryMock = new Mock<IFinanceiroRepository>();

            _financeiroRepositoryMock.Setup(x => x.UnitOfWork.Commit()).ReturnsAsync(true);

            _handler = new FinanceiroCommandHandler(_mediatorHandlerMock.Object, _financeiroRepositoryMock.Object);
        }

        [Test]
        public async Task Deve_Realizar_Pagamento_Com_Cartao_Valido()
        {
            var command = new PagamentoMatriculaCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Titular Teste",
                "9999999999999999",
                "123",
                DateTime.Now.AddYears(2),
                59
            );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result);
            _financeiroRepositoryMock.Verify(x => x.Adicionar(It.IsAny<Pagamento>()), Times.Once);
            _mediatorHandlerMock.Verify(x => x.PublicarEvento(It.IsAny<PagamentoMatriculaEvent>()), Times.Once);
        }

        [Test]
        public async Task Deve_Realizar_Pagamento_Com_Cartao_Invalido()
        {
            var command = new PagamentoMatriculaCommand(
                 Guid.NewGuid(),
                 Guid.NewGuid(),
                 "Titular Teste",
                 "9999999999999997",
                 "123",
                 DateTime.Now.AddYears(2),
                 59
             );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result);
            _financeiroRepositoryMock.Verify(x => x.Adicionar(It.IsAny<Pagamento>()), Times.Once);
            _mediatorHandlerMock.Verify(x => x.PublicarEvento(It.Is<PagamentoMatriculaEvent>(
                e => e.Status == nameof(StatusPagamentoEnum.REJEITADO))), Times.Once);
        }

        [Test]
        public async Task Nao_Deve_Realizar_Pagamento_Se_Comando_For_Invalido()
        {
            var command = new PagamentoMatriculaCommand(
                Guid.Empty,
                Guid.Empty,
                "Titular Teste",
                "9999999999999994",
                "123",
                DateTime.Now.AddYears(2),
                59
            );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result);
            _financeiroRepositoryMock.Verify(x => x.Adicionar(It.IsAny<Pagamento>()), Times.Never);
            _mediatorHandlerMock.Verify(x => x.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce);
        }
    }
}
