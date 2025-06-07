using Moq;
using PlataformaEducacional.Conteudo.Domain;
using PlataformaEducacional.Conteudo.Domain.Interfaces;
using PlataformaEducacional.Conteudo.Domain.Services;
using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.Conteudo.Tests
{
    public class ConteudoServiceTests
    {
        private ICursoService _cursoService;
        private Mock<ICursoRepository> _cursoRepository;

        private readonly Guid _cursoId = Guid.Parse("c150ec53-1866-4b40-8133-521dbd475492");

        [SetUp]
        public void Setup()
        {
            _cursoRepository = new Mock<ICursoRepository>();

            var cursos = new List<Curso>
            {
                NovoCurso(_cursoId, "Teste 01", true, "40"),
                NovoCurso(Guid.NewGuid(), "Teste 02", true, "20"),
                NovoCurso(Guid.NewGuid(), "Teste 03", true, "15"),
                NovoCurso(Guid.NewGuid(), "Teste 04", true, "25"),
            };

            _cursoRepository.Setup(x => x.ObterTodos()).ReturnsAsync(cursos.AsQueryable());

            _cursoRepository.Setup(x => x.ObterPorId(_cursoId))
                .ReturnsAsync(NovoCurso(_cursoId, "Teste 01", true, "40"));

            _cursoRepository.Setup(x => x.UnitOfWork.Commit()).ReturnsAsync(true);

            _cursoService = new CursoService(_cursoRepository.Object);
        }

        private Curso NovoCurso(Guid id, string nome, bool ativo, string cargaHoraria)
        {
            return new Curso(id, nome, ativo,
                new ConteudoProgramatico("Descrição teste", "", cargaHoraria));
        }

        [Test]
        public async Task Deve_Buscar_Todos_Os_Cursos()
        {
            var cursos = await _cursoService.FiltrarCursos(null, null);

            Assert.AreEqual(4, cursos.Count());
        }

        [Test]
        public async Task Deve_Buscar_Curso_Por_Id()
        {
            var curso = await _cursoService.BuscarCurso(_cursoId);

            Assert.AreEqual(_cursoId, curso.Id);
        }

        [Test]
        public void Deve_Adicionar_Curso()
        {
            var curso = NovoCurso(Guid.NewGuid(), "Teste 07", true, "39");

            Assert.DoesNotThrowAsync(() => _cursoService.AdicionarCurso(curso));
        }

        [Test]
        public async Task Deve_Alterar_Status_Do_Curso()
        {
            var curso = NovoCurso(_cursoId, "Curso Status", true, "10");
            _cursoRepository.Setup(r => r.ObterPorId(_cursoId)).ReturnsAsync(curso);

            await _cursoService.AlterarStatusCurso(_cursoId, false);

            Assert.IsFalse(curso.Ativo);
            _cursoRepository.Verify(r => r.Atualizar(It.Is<Curso>(c => c.Id == _cursoId && !c.Ativo)), Times.Once);
            _cursoRepository.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Test]
        public void Nao_Deve_Alterar_Status_Se_Curso_Nao_Existir()
        {
            _cursoRepository.Setup(r => r.ObterPorId(_cursoId)).ReturnsAsync((Curso)null);

            var ex = Assert.ThrowsAsync<DomainException>(() => _cursoService.AlterarStatusCurso(_cursoId, false));
            Assert.That(ex.Message, Is.EqualTo("Curso não encontrado"));
        }

        [Test]
        public async Task Deve_Atualizar_Curso()
        {
            var cursoDb = NovoCurso(_cursoId, "Curso Original", true, "20");
            var novoConteudo = new ConteudoProgramatico("Descrição atualizada", "", "40");
            var cursoNovo = new Curso(_cursoId, "Curso Atualizado", false, novoConteudo);

            _cursoRepository.Setup(r => r.ObterPorId(_cursoId)).ReturnsAsync(cursoDb);

            await _cursoService.AtualizarCurso(_cursoId, cursoNovo);

            Assert.AreEqual("Curso Atualizado", cursoDb.Nome);
            Assert.IsFalse(cursoDb.Ativo);
            Assert.AreEqual("Descrição atualizada", cursoDb.Conteudo.Descricao);
            Assert.AreEqual("40", cursoDb.Conteudo.CargaHoraria);

            _cursoRepository.Verify(r => r.Atualizar(It.IsAny<Curso>()), Times.Once);
            _cursoRepository.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Test]
        public void Nao_Deve_Atualizar_Curso_Se_Nao_Existir()
        {
            _cursoRepository.Setup(r => r.ObterPorId(_cursoId)).ReturnsAsync((Curso)null);

            var cursoNovo = NovoCurso(_cursoId, "Atualizado", false, "40");

            var ex = Assert.ThrowsAsync<DomainException>(() => _cursoService.AtualizarCurso(_cursoId, cursoNovo));
            Assert.That(ex.Message, Is.EqualTo("Curso não encontrado"));
        }

        [Test]
        public async Task Deve_Adicionar_Aula_Ao_Curso()
        {
            var curso = NovoCurso(_cursoId, "Curso com aula", true, "40");
            var novaAula = new Aula(Guid.NewGuid(), "Nova Aula", "1", _cursoId, true);

            _cursoRepository.Setup(r => r.ObterPorId(_cursoId)).ReturnsAsync(curso);

            var resultado = await _cursoService.AdicionarAula(_cursoId, novaAula);

            Assert.IsTrue(resultado);
            Assert.Contains(novaAula, curso.Aulas.ToList());
            _cursoRepository.Verify(r => r.Atualizar(curso), Times.Once);
            _cursoRepository.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Test]
        public void AdicionarAula_Deve_LancarExcecao_Quando_CursoNaoEncontrado()
        {
            var novaAula = new Aula(Guid.NewGuid(), "Nova Aula", "1", _cursoId, true);

            _cursoRepository.Setup(r => r.ObterPorId(_cursoId)).ReturnsAsync((Curso)null!);

            var ex = Assert.ThrowsAsync<DomainException>(() => _cursoService.AdicionarAula(_cursoId, novaAula));

            Assert.AreEqual("Curso não encontrado", ex.Message);
        }

        [Test]
        public async Task Deve_Atualizar_Aula_Do_Curso()
        {
            var aulaId = Guid.NewGuid();
            var curso = NovoCurso(_cursoId, "Curso com aula", true, "40");

            var aulaOriginal = new Aula(aulaId, "Aula Antiga", "35", _cursoId, true);
            curso.AdicionarAula(aulaOriginal);

            var aulaAtualizada = new Aula(aulaId, "Aula Atualizada", "20", _cursoId, true);

            _cursoRepository.Setup(r => r.ObterPorId(_cursoId)).ReturnsAsync(curso);

            var resultado = await _cursoService.AtualizarAula(_cursoId, aulaId, aulaAtualizada);

            Assert.IsTrue(resultado);

            var aula = curso.Aulas.First(a => a.Id == aulaId);
            Assert.AreEqual("Aula Atualizada", aula.Titulo);
            Assert.AreEqual("20", aula.Duracao);
            Assert.AreEqual(true, aula.Ativo);

            _cursoRepository.Verify(r => r.Atualizar(curso), Times.Once);
            _cursoRepository.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Test]
        public void AtualizarAula_Deve_LancarExcecao_Quando_CursoNaoEncontrado()
        {
            var aulaId = Guid.NewGuid();
            var aulaAtualizada = new Aula(aulaId, "Aula Atualizada", "20", _cursoId, true);

            _cursoRepository.Setup(r => r.ObterPorId(_cursoId)).ReturnsAsync((Curso)null!);

            var ex = Assert.ThrowsAsync<DomainException>(() => _cursoService.AtualizarAula(_cursoId, aulaId, aulaAtualizada));

            Assert.AreEqual("Curso não encontrado", ex.Message);
        }

        [Test]
        public async Task Deve_Remover_Aula_Do_Curso()
        {
            var aulaId = Guid.NewGuid();
            var curso = NovoCurso(_cursoId, "Curso com aula", true, "40");
            var aula = new Aula(aulaId, "Aula a Remover", "15", _cursoId, true);
            curso.AdicionarAula(aula);

            _cursoRepository.Setup(r => r.ObterPorId(_cursoId)).ReturnsAsync(curso);

            var resultado = await _cursoService.RemoverAula(_cursoId, aulaId);

            Assert.IsTrue(resultado);
            Assert.IsFalse(curso.Aulas.Any(a => a.Id == aulaId));

            _cursoRepository.Verify(r => r.Atualizar(curso), Times.Once);
            _cursoRepository.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Test]
        public void RemoverAula_Deve_LancarExcecao_Quando_CursoNaoEncontrado()
        {
            var aulaId = Guid.NewGuid();

            _cursoRepository.Setup(r => r.ObterPorId(_cursoId)).ReturnsAsync((Curso)null!);

            var ex = Assert.ThrowsAsync<DomainException>(() => _cursoService.RemoverAula(_cursoId, aulaId));

            Assert.AreEqual("Curso não encontrado", ex.Message);
        }
    }
}
