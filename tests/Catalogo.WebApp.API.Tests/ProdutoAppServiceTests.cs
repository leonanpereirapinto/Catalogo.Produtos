using System;
using System.Threading.Tasks;
using AutoMapper;
using Catalogo.Domain.Interfaces;
using Catalogo.Domain.Models;
using Moq;
using Moq.AutoMock;
using WebApp.API.AutoMapper;
using WebApp.API.Services;
using WebApp.API.ViewModels;
using Xunit;

namespace Catalogo.WebApp.API.Tests
{
    public class ProdutoAppServiceTests
    {
        private readonly AutoMocker _mock;
        private readonly Mock<IProdutoRepository> _repositorioMock;
        private readonly ProdutoAppService _sut;

        public ProdutoAppServiceTests()
        {
            _mock = new AutoMocker();
            _mock.Use(GetMapper());

            _repositorioMock = _mock.GetMock<IProdutoRepository>();
            _sut = _mock.CreateInstance<ProdutoAppService>();
        }

        [Fact(DisplayName = "Deve chamar Inserir e SaveChangesAsync do ProdutoRepository")]
        [Trait("Categoria", "Catalogo.Application.ProdutoService")]
        public async Task ProdutoService_CriarProduto_DeveChamarInserirESaveChangesAsyncDoProdutoRepository()
        {
            var nome = "Produto de teste";
            var estoque = 20;
            var valor = 200;
            var produtoViewModel = ObterCriarProdutoViewModel(nome, estoque, valor);

            await _sut.CriarProduto(produtoViewModel);

            _repositorioMock.Verify(i => i.Inserir(
                It.Is<Produto>(p => p.Nome == nome && p.Estoque == estoque && p.Valor == valor)), 
                Times.Once);

            _repositorioMock.Verify(i => i.SaveChangesAsync(), Times.Once);
        }

        [Fact(DisplayName = "Deve atualizar o produto ao receber um ProdutoViewModel válido")]
        [Trait("Categoria", "Catalogo.Application.ProdutoService")]
        public async Task ProdutoService_AtualizarProduto_DeveAtualizarOProdutoAoReceberProdutoValido()
        {
            var produtoId = Guid.NewGuid();
            var nome = "Nome novo";
            var estoque = 99;
            var valor = 999;

            var produtoViewModel = ObterProdutoViewModel(produtoId, nome, estoque, valor);

            _repositorioMock.Setup(r => r.ExisteProdutoComId(produtoId)).ReturnsAsync(true);

            await _sut.AtualizarProduto(produtoViewModel);

            _repositorioMock.Verify(i => i.Atualizar(It.Is<Produto>(p => p.Id == produtoId && p.Nome == nome && p.Estoque == estoque && p.Valor == valor)), Times.Once);

            _repositorioMock.Verify(i => i.SaveChangesAsync(), Times.Once);
        }
        
        [Fact(DisplayName = "AtualizarProduto deve retornar mensagem de erro se ProdutoViewModel id for vazio ou nulo")]
        [Trait("Categoria", "Catalogo.Application.ProdutoService")]
        public async Task ProdutoService_AtualizarProduto_DeveRetornarMensagemDeErroSeIdForVazioOuNulo()
        {
            var produtoViewModel = ObterProdutoViewModel();
            produtoViewModel.Id = null;

            var (sucesso, mensagemErro) = await _sut.AtualizarProduto(produtoViewModel);

            Assert.False(sucesso);
            Assert.Equal("Id do produto é obrigatório", mensagemErro);
        }   
        
        [Fact(DisplayName = "AtualizarProduto deve retornar mensagem de erro se ObterPeloId retornar nulo")]
        [Trait("Categoria", "Catalogo.Application.ProdutoService")]
        public async Task ProdutoService_AtualizarProduto_DeveRetornarMensagemDeErroSeObterPeloIdRetornarNulo()
        {
            var produtoId = Guid.NewGuid();
            var produtoViewModel = ObterProdutoViewModel(id: produtoId);
            _repositorioMock.Setup(r => r.ExisteProdutoComId(It.IsAny<Guid>())).ReturnsAsync(false);

            var (sucesso, mensagemErro) = await _sut.AtualizarProduto(produtoViewModel);

            Assert.False(sucesso);
            Assert.Equal($"Produto com o Id {produtoId} não foi encontrado", mensagemErro);
        }

        [Fact(DisplayName = "RemoverProduto deve retornar mensagem de erro se o produto não for encontrado")]
        [Trait("Categoria", "Catalogo.Application.ProdutoService")]
        public async Task ProdutoService_RemoverProduto_DeveRetornarUmaMensagemDeErroSeProdutoNaoForEncontrado()
        {
            var produtoId = Guid.NewGuid();
            _repositorioMock.Setup(r => r.ExisteProdutoComId(produtoId)).ReturnsAsync(false);

            var (sucesso, mensagemErro) = await _sut.RemoverProduto(produtoId);

            Assert.False(sucesso);
            Assert.Equal($"Produto com o Id {produtoId} não foi encontrado", mensagemErro);
        }

        [Fact(DisplayName = "RemoverProduto deve remover o produto e salvar as alterações")]
        [Trait("Categoria", "Catalogo.Application.ProdutoService")]
        public async Task ProdutoService_RemoverProduto_DeveRemoverProdutoESalvarAsAlteracoes()
        {
            var produtoId = Guid.NewGuid();
            _repositorioMock.Setup(r => r.ExisteProdutoComId(produtoId)).ReturnsAsync(true);
            _repositorioMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var (sucesso, mensagemErro) = await _sut.RemoverProduto(produtoId);

            Assert.True(sucesso);
            Assert.True(string.IsNullOrEmpty(mensagemErro));
            _repositorioMock.Verify(i => i.Deletar(produtoId), Times.Once);
            _repositorioMock.Verify(i => i.SaveChangesAsync(), Times.Once);
        }

        private IMapper GetMapper()
        {
            var configuration = new MapperConfiguration(c => c.AddMaps(typeof(DomainToViewModelMappingProfile), typeof(ViewModelToDomainMappingProfile)));
            return new Mapper(configuration);
        }

        private CriarProdutoViewModel ObterCriarProdutoViewModel(string nome = "Nome do Produto", int estoque = 10, decimal valor = 100)
        {
            return new CriarProdutoViewModel
            {
                Nome = nome,
                Estoque = estoque,
                Valor = valor
            };
        }

        private ProdutoViewModel ObterProdutoViewModel(Guid? id = null, string nome = "Nome do Produto", int estoque = 10, decimal valor = 100)
        {
            id ??= Guid.NewGuid();
            return new ProdutoViewModel
            {
                Id = id,
                Nome = nome,
                Estoque = estoque,
                Valor = valor
            };
        }
    }
}