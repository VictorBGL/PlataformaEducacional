using Moq;
using PlataformaEducacional.Aluno.AntiCorruption;
using PlataformaEducacional.Aluno.Application.Commands;
using PlataformaEducacional.Aluno.Domain;
using PlataformaEducacional.Core.Communication;
using PlataformaEducacional.Core.DomainObjects;
using PlataformaEducacional.Core.Enums;


namespace PlataformaEducacional.Aluno.Tests
{
    public class AlunoCommandTests
    {
        private Mock<IConteudoService> _conteudoServiceMock;
        private Mock<IAlunoRepository> _alunoRepositoryMock;
        private Mock<IMediatorHandler> _mediatorHandler;

        private AlunoCommandHandler _alunoCommandHandler;


        private Guid _cursoId = Guid.NewGuid();
        private Guid _aulaId = Guid.NewGuid();
        private Guid _alunoId = Guid.Parse("e03b5aa6-4523-40b5-8806-1ce680d5dde7");

        [SetUp]
        public void Setup()
        {
            _conteudoServiceMock = new Mock<IConteudoService>();
            _conteudoServiceMock.Setup(x => x.BuscarQuantidadeAulasCurso(It.IsAny<Guid>()))
                .ReturnsAsync(20);
            _conteudoServiceMock.Setup(x => x.BuscarNomeCurso(It.IsAny<Guid>()))
                .ReturnsAsync("Curso de Testes");

            _alunoRepositoryMock = new Mock<IAlunoRepository>();

            _alunoRepositoryMock.Setup(x => x.ObterPorId(_alunoId))
                .ReturnsAsync(() =>
                {
                    return new Domain.Aluno(_alunoId, "Victor Lino", "victor@aluno.com", DateTime.Now.AddYears(-20));
                });

            _alunoRepositoryMock.Setup(x => x.Atualizar(It.IsAny<Domain.Aluno>()));
            _alunoRepositoryMock.Setup(x => x.UnitOfWork.Commit())
                .ReturnsAsync(true);

            _mediatorHandler = new Mock<IMediatorHandler>();

            _alunoCommandHandler = new AlunoCommandHandler(
                _mediatorHandler.Object,
                _alunoRepositoryMock.Object,
                _conteudoServiceMock.Object
            );
        }

        [Test]
        public async Task CadastrarAluno_DeveRetornarTrue_QuandoComandoValido()
        {
            var command = new CadastrarAlunoCommand(Guid.NewGuid(), "Victor Lino", "victor@teste.com", DateTime.Now.AddYears(-20));

            var resultado = await _alunoCommandHandler.Handle(command, CancellationToken.None);

            Assert.IsTrue(resultado);
            _alunoRepositoryMock.Verify(r => r.Adicionar(It.IsAny<Domain.Aluno>()), Times.Once);
            _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Test]
        public async Task Matricula_RetornarTrue_QuandoComandoValido()
        {
            var command = new RealizarMatriculaCommand(_cursoId, _alunoId);

            var resultado = await _alunoCommandHandler.Handle(command, CancellationToken.None);

            Assert.IsTrue(resultado);
            _alunoRepositoryMock.Verify(r => r.Atualizar(It.IsAny<Domain.Aluno>()), Times.Once);
            _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Test]
        public void Matricula_LancarExcecao_QuandoAlunoNaoEncontrado()
        {
            var command = new RealizarMatriculaCommand(_cursoId, Guid.NewGuid());
            _alunoRepositoryMock.Setup(r => r.ObterPorId(It.IsAny<Guid>()))
                .ReturnsAsync((Domain.Aluno)null);

            var ex = Assert.ThrowsAsync<DomainException>(async () =>
                await _alunoCommandHandler.Handle(command, CancellationToken.None));

            Assert.AreEqual("Aluno não encontrado.", ex.Message);
        }

        [Test]
        public void Matricula_LancarExcecao_QuandoJaPossuiMatricula()
        {
            var alunoComMatricula = new Domain.Aluno(_alunoId, "Teste", "teste@test.com", DateTime.Now.AddYears(-20));
            var matricula = new Matricula(_alunoId, _cursoId);
            alunoComMatricula.AdicionarMatricula(matricula);

            _alunoRepositoryMock.Setup(r => r.ObterPorId(_alunoId))
                .ReturnsAsync(alunoComMatricula);

            var command = new RealizarMatriculaCommand(_cursoId, _alunoId);

            var ex = Assert.ThrowsAsync<DomainException>(async () =>
                await _alunoCommandHandler.Handle(command, CancellationToken.None));

            Assert.AreEqual("Existe matrícula realizada para este curso.", ex.Message);
        }

        [Test]
        public async Task FinalizarAula_DeveRetornarTrue_QuandoTudoOk()
        {
            var aluno = new Domain.Aluno(_alunoId, "Teste", "teste@test.com", DateTime.Now.AddYears(-20));
            var matricula = new Matricula(_alunoId, _cursoId);
            matricula.AtualizarStatus(StatusMatriculaEnum.ATIVO);
            aluno.AdicionarMatricula(matricula);

            _alunoRepositoryMock.Setup(r => r.ObterPorId(_alunoId))
                .ReturnsAsync(aluno);

            _alunoRepositoryMock.Setup(r => r.BuscarMatricula(_alunoId, _cursoId))
                .ReturnsAsync(matricula);

            _conteudoServiceMock.Setup(s => s.BuscarQuantidadeAulasCurso(_cursoId))
                .ReturnsAsync(10);

            var command = new FinalizarAulaCommand(_alunoId, _cursoId, _aulaId);

            var result = await _alunoCommandHandler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result);
        }

        [Test]
        public void FinalizarAula_DeveLancarExcecao_QuandoAlunoNaoEncontrado()
        {
            var command = new FinalizarAulaCommand(_cursoId, Guid.NewGuid(), _aulaId);

            _alunoRepositoryMock.Setup(r => r.ObterPorId(It.IsAny<Guid>()))
                .ReturnsAsync((Domain.Aluno)null);

            var ex = Assert.ThrowsAsync<DomainException>(async () =>
                await _alunoCommandHandler.Handle(command, CancellationToken.None));

            Assert.AreEqual("Aluno não encontrado.", ex.Message);
        }

        [Test]
        public void FinalizarAula_DeveLancarExcecao_QuandoMatriculaNaoEncontrada()
        {
            var aluno = new Domain.Aluno(_alunoId, "Victor Lino", "victor@teste.com", DateTime.Now.AddYears(-20));

            _alunoRepositoryMock.Setup(r => r.ObterPorId(_alunoId))
                .ReturnsAsync(aluno);

            _alunoRepositoryMock.Setup(r => r.BuscarMatricula(_alunoId, _cursoId))
                .ReturnsAsync((Matricula)null);

            var command = new FinalizarAulaCommand(_alunoId, _cursoId, _aulaId);

            var ex = Assert.ThrowsAsync<DomainException>(async () =>
                await _alunoCommandHandler.Handle(command, CancellationToken.None));

            Assert.AreEqual("Matrícula não encontrada para este curso.", ex.Message);
        }

        [Test]
        public void FinalizarAula_DeveLancarExcecao_QuandoMatriculaPendente()
        {
            var aluno = new Domain.Aluno(_alunoId, "Victor Lino", "victor@teste.com", DateTime.Now.AddYears(-20));

            var matricula = new Matricula(_alunoId, _cursoId);
            matricula.AtualizarStatus(StatusMatriculaEnum.PENDENTE);
            aluno.AdicionarMatricula(matricula);

            _alunoRepositoryMock.Setup(r => r.ObterPorId(_alunoId))
                .ReturnsAsync(aluno);

            _alunoRepositoryMock.Setup(r => r.BuscarMatricula(_alunoId, _cursoId))
                .ReturnsAsync(matricula);

            var command = new FinalizarAulaCommand(_alunoId, _cursoId, _aulaId);

            var ex = Assert.ThrowsAsync<DomainException>(async () =>
                await _alunoCommandHandler.Handle(command, CancellationToken.None));

            Assert.AreEqual("Não é possível concluir a aula sem efetuar o pagamento.", ex.Message);
        }

        [Test]
        public void FinalizarAula_DeveLancarExcecao_QuandoCursoConcluido()
        {
            var aluno = new Domain.Aluno(_alunoId, "Victor Lino", "victor@teste.com", DateTime.Now.AddYears(-20));

            var matricula = new Matricula(_alunoId, _cursoId);
            matricula.AtualizarStatus(StatusMatriculaEnum.CONCLUIDO);
            aluno.AdicionarMatricula(matricula);

            _alunoRepositoryMock.Setup(r => r.ObterPorId(_alunoId))
                .ReturnsAsync(aluno);

            _alunoRepositoryMock.Setup(r => r.BuscarMatricula(_alunoId, _cursoId))
                .ReturnsAsync(matricula);

            var command = new FinalizarAulaCommand(_alunoId, _cursoId, _aulaId);

            var ex = Assert.ThrowsAsync<DomainException>(async () =>
                await _alunoCommandHandler.Handle(command, CancellationToken.None));

            Assert.AreEqual("Não é possível concluir a aula, pois o curso já foi concluído.", ex.Message);
        }

        [Test]
        public async Task EmitirCertificado_DeveEmitir_QuandoProgressoCompleto()
        {
            var aluno = new Domain.Aluno(_alunoId, "Victor Lino", "victor@teste.com", DateTime.Now.AddYears(-20));
            var matricula = new Matricula(_alunoId, _cursoId);
            matricula.AtualizarStatus(StatusMatriculaEnum.ATIVO);
            matricula.Historico.AtualizarProgresso(100);
            aluno.AdicionarMatricula(matricula);

            _alunoRepositoryMock.Setup(r => r.ObterPorId(_alunoId))
                .ReturnsAsync(aluno);

            _alunoRepositoryMock.Setup(r => r.BuscarMatricula(_alunoId, _cursoId))
                .ReturnsAsync(matricula);

            _conteudoServiceMock.Setup(a => a.BuscarNomeCurso(_cursoId)).ReturnsAsync("Curso Teste");

            var command = new EmitirCertificadoCommand(_cursoId, _alunoId);

            var result = await _alunoCommandHandler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result);
            Assert.IsNotNull(aluno.Certificados);
            Assert.AreEqual(StatusMatriculaEnum.CONCLUIDO, matricula.Status);
        }

        [Test]
        public void EmitirCertificado_DeveLancarExcecao_QuandoAlunoNaoEncontrado()
        {
            _alunoRepositoryMock.Setup(r => r.ObterPorId(_alunoId))
                .ReturnsAsync((Domain.Aluno)null);

            var command = new EmitirCertificadoCommand(_cursoId, _alunoId);

            var ex = Assert.ThrowsAsync<DomainException>(async () =>
                await _alunoCommandHandler.Handle(command, CancellationToken.None));

            Assert.AreEqual("Aluno não encontrado.", ex.Message);
        }

        [Test]
        public void EmitirCertificado_DeveLancarExcecao_QuandoMatriculaNaoEncontrada()
        {
            var aluno = new Domain.Aluno(_alunoId, "Victor Lino", "victor@teste.com", DateTime.Now.AddYears(-20));

            _alunoRepositoryMock.Setup(r => r.ObterPorId(_alunoId))
                .ReturnsAsync(aluno);

            var command = new EmitirCertificadoCommand(_cursoId, _alunoId);

            var ex = Assert.ThrowsAsync<DomainException>(async () =>
                await _alunoCommandHandler.Handle(command, CancellationToken.None));

            Assert.AreEqual("Matrícula não encontrada para este curso.", ex.Message);
        }

        [Test]
        public void EmitirCertificado_DeveLancarExcecao_QuandoProgressoIncompleto()
        {
            var aluno = new Domain.Aluno(_alunoId, "Victor Lino", "victor@teste.com", DateTime.Now.AddYears(-20));
            var matricula = new Matricula(_alunoId, _cursoId);
            matricula.AtualizarStatus(StatusMatriculaEnum.ATIVO);
            matricula.Historico.AtualizarProgresso(80);
            aluno.AdicionarMatricula(matricula);

            _alunoRepositoryMock.Setup(r => r.ObterPorId(_alunoId))
                .ReturnsAsync(aluno);

            _alunoRepositoryMock.Setup(r => r.BuscarMatricula(_alunoId, _cursoId))
                .ReturnsAsync(matricula);

            var command = new EmitirCertificadoCommand(_cursoId, _alunoId);

            var ex = Assert.ThrowsAsync<DomainException>(async () =>
                await _alunoCommandHandler.Handle(command, CancellationToken.None));

            Assert.AreEqual($"O aluno não finalizou todas as aulas do curso, progresso atual 80%", ex.Message);
        }
    }
}
