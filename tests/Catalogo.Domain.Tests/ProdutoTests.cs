using Catalogo.Core.Exceptions;
using Catalogo.Domain.Entities;
using Xunit;

namespace Catalogo.Domain.Tests
{
    public class ProdutoTests
    {
        [Fact(DisplayName = "Criar produto ao receber informações válidas")]
        [Trait("Categoria", "Catalogo.Produto")]
        public void Produto_Constructor_DeveCriarProdutoComInformacoesValidas()
        {
            var nome = "Produto de Teste";
            var estoque = 9;
            var valor = 99;

            var produto = CriarProdutoValido(nome, estoque, valor);

            Assert.Equal(nome, produto.Nome);
            Assert.Equal(estoque, produto.Estoque);
            Assert.Equal(valor, produto.Valor);
        }

        [Fact(DisplayName = "Lançar DomainException se Nome for inválido")]
        [Trait("Categoria", "Catalogo.Produto")]
        public void Produto_Constructor_DeveLancarDomainExceptionSeNomeForInvalido()
        {
            var stringVaziaException = Assert.Throws<DomainException>(() => CriarProdutoValido(nome: ""));
            var stringNullException = Assert.Throws<DomainException>(() => CriarProdutoValido(nome: null));
            
            Assert.Equal("O campo Nome é obrigatório", stringVaziaException.Message);
            Assert.Equal("O campo Nome é obrigatório", stringNullException.Message);
        }
        
        [Fact(DisplayName = "Deve lançar DomainException se Valor for negativo")]
        [Trait("Categoria", "Catalogo.Produto")]
        public void Produto_Constructor_DeveLancarDomainExceptionSeValorForNegativo()
        {
            var exception = Assert.Throws<DomainException>(() => CriarProdutoValido(valor: -1));
            
            Assert.Equal("O Valor não pode ser negativo", exception.Message);
        }

        private Produto CriarProdutoValido(string nome = "Nome do Produto", int estoque = 10, decimal valor = 100)
        {
            return new Produto(nome, estoque, valor);
        }
    }
}