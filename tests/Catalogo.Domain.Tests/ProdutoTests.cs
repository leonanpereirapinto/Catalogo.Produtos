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

        private Produto CriarProdutoValido(string nome = "Nome do Produto", int estoque = 10, decimal valor = 100)
        {
            return new Produto(nome, estoque, valor);
        }
    }
}