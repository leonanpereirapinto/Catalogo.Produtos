using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Catalogo.Domain.Enums;
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
        [Trait("Categoria", "Catalogo.Application.ProdutoAppService")]
        public async Task ProdutoAppService_CriarProduto_DeveChamarInserirESaveChangesAsyncDoProdutoRepository()
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
        [Trait("Categoria", "Catalogo.Application.ProdutoAppService")]
        public async Task ProdutoAppService_AtualizarProduto_DeveAtualizarOProdutoAoReceberProdutoValido()
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
        [Trait("Categoria", "Catalogo.Application.ProdutoAppService")]
        public async Task ProdutoAppService_AtualizarProduto_DeveRetornarMensagemDeErroSeIdForVazioOuNulo()
        {
            var produtoViewModel = ObterProdutoViewModel();
            produtoViewModel.Id = null;

            var (sucesso, mensagemErro) = await _sut.AtualizarProduto(produtoViewModel);

            Assert.False(sucesso);
            Assert.Equal("Id do produto é obrigatório", mensagemErro);
        }   
        
        [Fact(DisplayName = "AtualizarProduto deve retornar mensagem de erro se ObterPeloId retornar nulo")]
        [Trait("Categoria", "Catalogo.Application.ProdutoAppService")]
        public async Task ProdutoAppService_AtualizarProduto_DeveRetornarMensagemDeErroSeObterPeloIdRetornarNulo()
        {
            var produtoId = Guid.NewGuid();
            var produtoViewModel = ObterProdutoViewModel(id: produtoId);
            _repositorioMock.Setup(r => r.ExisteProdutoComId(It.IsAny<Guid>())).ReturnsAsync(false);

            var (sucesso, mensagemErro) = await _sut.AtualizarProduto(produtoViewModel);

            Assert.False(sucesso);
            Assert.Equal($"Produto com o Id {produtoId} não foi encontrado", mensagemErro);
        }

        [Fact(DisplayName = "RemoverProduto deve retornar mensagem de erro se o produto não for encontrado")]
        [Trait("Categoria", "Catalogo.Application.ProdutoAppService")]
        public async Task ProdutoAppService_RemoverProduto_DeveRetornarUmaMensagemDeErroSeProdutoNaoForEncontrado()
        {
            var produtoId = Guid.NewGuid();
            _repositorioMock.Setup(r => r.ExisteProdutoComId(produtoId)).ReturnsAsync(false);

            var (sucesso, mensagemErro) = await _sut.RemoverProduto(produtoId);

            Assert.False(sucesso);
            Assert.Equal($"Produto com o Id {produtoId} não foi encontrado", mensagemErro);
        }

        [Fact(DisplayName = "RemoverProduto deve remover o produto e salvar as alterações")]
        [Trait("Categoria", "Catalogo.Application.ProdutoAppService")]
        public async Task ProdutoAppService_RemoverProduto_DeveRemoverProdutoESalvarAsAlteracoes()
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
        
        [Fact(DisplayName = "ObterTodos deve buscar pelos Ids")]
        [Trait("Categoria", "Catalogo.Application.ProdutoAppService")]
        public async Task ProdutoAppService_ObterTodos_DeveBuscarPelosIds()
        {
            var listaIdsConvertidos = new List<Guid>()
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            var listaDeProdutos = new List<Produto>
            {
                new ("Produto1", 10, 100) { Id = listaIdsConvertidos[0] },
                new ("Produto2", 20, 200) { Id = listaIdsConvertidos[1] },
                new ("Produto3", 30, 300) { Id = listaIdsConvertidos[2] },
            };

            _repositorioMock
                .Setup(r => r.ObterTodos(listaIdsConvertidos, null, null, null))
                .ReturnsAsync(listaDeProdutos);

            var obterTodosViewModel = new ObterTodosViewModel
            {
                Id = string.Join(",", listaIdsConvertidos)
            };

            var (produtoViewModels, mensagemErro) = await _sut.ObterTodos(obterTodosViewModel);

            Assert.NotNull(produtoViewModels);
            Assert.True(string.IsNullOrEmpty(mensagemErro));
            _repositorioMock.Verify(r => r.ObterTodos(listaIdsConvertidos, null, null, null), Times.Once);
        }

        [Fact(DisplayName = "ObterTodos deve buscar pelos Nomes")]
        [Trait("Categoria", "Catalogo.Application.ProdutoAppService")]
        public async Task ProdutoAppService_ObterTodos_DeveBuscarPelosNomes()
        {
            var listaNomes = new List<string>
            {
                "Produto1",
                "Produto2",
                "Produto3"
            };
            OrdenarPor? ordenarPorEnum = null;
            Ordenacao? ordenacaoEnum = null;

            var listaDeProdutos = new List<Produto>
            {
                new (listaNomes[0], 10, 100) { Id = Guid.NewGuid() },
                new (listaNomes[1], 20, 200) { Id = Guid.NewGuid() },
                new (listaNomes[2], 30, 300) { Id = Guid.NewGuid() },
            };

            _repositorioMock
                .Setup(r => r.ObterTodos(null, listaNomes, ordenarPorEnum, ordenacaoEnum))
                .ReturnsAsync(listaDeProdutos);

            var obterTodosViewModel = new ObterTodosViewModel
            {
                Nome = string.Join(",", listaNomes)
            };

            var (produtoViewModels, mensagemErro) = await _sut.ObterTodos(obterTodosViewModel);

            Assert.NotNull(produtoViewModels);
            Assert.True(string.IsNullOrEmpty(mensagemErro));
            _repositorioMock.Verify(r => r.ObterTodos(null, listaNomes, ordenarPorEnum, ordenacaoEnum), Times.Once);
        }

        [Theory(DisplayName = "ObterTodos deve ordenar por diferentes campos")]
        [Trait("Categoria", "Catalogo.Application.ProdutoAppService")]
        [InlineData("nome:asc", OrdenarPor.Nome, Ordenacao.Asc)]
        [InlineData("nome:desc", OrdenarPor.Nome, Ordenacao.Desc)]
        [InlineData("estoque:asc", OrdenarPor.Estoque, Ordenacao.Asc)]
        [InlineData("estoque:desc", OrdenarPor.Estoque, Ordenacao.Desc)]
        [InlineData("valor:asc", OrdenarPor.Valor, Ordenacao.Asc)]
        [InlineData("valor:desc", OrdenarPor.Valor, Ordenacao.Desc)]
        public async Task ProdutoAppService_ObterTodos_DeveOrdenarPorDiferentesCampos(string ordenarPor, OrdenarPor campo, Ordenacao ordenacao)
        {
            OrdenarPor? ordenarPorEnum = campo;
            Ordenacao? ordenacaoEnum = ordenacao;

            var listaDeProdutos = new List<Produto>
            {
                new ("Produto1", 10, 100) { Id = Guid.NewGuid() },
                new ("Produto2", 20, 200) { Id = Guid.NewGuid() },
                new ("Produto3", 30, 300) { Id = Guid.NewGuid() },
            };

            _repositorioMock
                .Setup(r => r.ObterTodos(null, null, ordenarPorEnum, ordenacaoEnum))
                .ReturnsAsync(listaDeProdutos);

            var obterTodosViewModel = new ObterTodosViewModel
            {
                OrdenarPor = ordenarPor,
            };

            var (produtoViewModels, mensagemErro) = await _sut.ObterTodos(obterTodosViewModel);

            Assert.NotNull(produtoViewModels);
            Assert.Equal(3, produtoViewModels.Count);
            Assert.Equal(listaDeProdutos[0].Id, produtoViewModels[0].Id);
            Assert.Equal(listaDeProdutos[1].Id, produtoViewModels[1].Id);
            Assert.Equal(listaDeProdutos[2].Id, produtoViewModels[2].Id);
            Assert.True(string.IsNullOrEmpty(mensagemErro));
            _repositorioMock.Verify(r => r.ObterTodos(null, null, ordenarPorEnum, ordenacaoEnum), Times.Once);
        }

        [Theory(DisplayName = "ObterTodos deve retornar uma mensagem de erro quando receber OrdenarPor inválido")]
        [Trait("Categoria", "Catalogo.Application.ProdutoAppService")]
        [InlineData("valorQualquer", "OrdenarPor possui um formato inválido")]
        [InlineData("invalido:asc", "A primeira parte do campo OrdenarPor possui um formato inválido")]
        [InlineData("invalido:desc", "A primeira parte do campo OrdenarPor possui um formato inválido")]
        [InlineData("nome:invalido", "A segunda parte do campo OrdenarPor possui um formato inválido")]
        [InlineData("estoque:invalido", "A segunda parte do campo OrdenarPor possui um formato inválido")]
        [InlineData("valor:invalido", "A segunda parte do campo OrdenarPor possui um formato inválido")]
        public async Task ProdutoAppService_ObterTodos_DeveRetornarUmaMensagemDeErroQuandoReceberOrdenarPorInvalido(string ordenarPor, string mensagemErroEsperada)
        {
            var obterTodosViewModel = new ObterTodosViewModel
            {
                OrdenarPor = ordenarPor,
            };

            var (produtoViewModels, mensagemErro) = await _sut.ObterTodos(obterTodosViewModel);

            Assert.Null(produtoViewModels);
            Assert.Equal(mensagemErroEsperada, mensagemErro);
        }

        [Theory(DisplayName = "ObterTodos deve retornar uma mensagem de erro quando receber Ids inválidos")]
        [Trait("Categoria", "Catalogo.Application.ProdutoAppService")]
        [InlineData("invalido", "Id com formato inválido fornecido")]
        [InlineData("aa73d66e-2a24-4eec-b5cb-03e99aa01e7c,invalido2", "Id com formato inválido fornecido")]
        [InlineData("invalido1,invalido2,invalido3", "Id com formato inválido fornecido")]
        public async Task ProdutoAppService_ObterTodos_DeveRetornarUmaMensagemDeErroQuandoReceberIdsInvalidos(string ids, string mensagemErroEsperada)
        {
            var obterTodosViewModel = new ObterTodosViewModel
            {
                Id = ids
            };

            var (produtoViewModels, mensagemErro) = await _sut.ObterTodos(obterTodosViewModel);

            Assert.Null(produtoViewModels);
            Assert.Equal(mensagemErroEsperada, mensagemErro);
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