using Catalogo.Core.Exceptions;
using Catalogo.Domain.Entities;
using Xunit;

namespace Catalogo.Domain.Tests
{
    public class ProdutoTests
    {
        [Fact(DisplayName = "Deve criar produto ao receber informações válidas")]
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

        [Fact(DisplayName = "Deve lançar DomainException se Nome for inválido")]
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

        [Fact(DisplayName = "Deve alterar o Nome do produto se o Nome for válido")]
        [Trait("Categoria", "Catalogo.Produto")]
        public void Produto_AlterarNome_DeveAlterarNomeDoProduto()
        {
            var produto = CriarProdutoValido();

            produto.AlterarNome("Alterado");

            Assert.Equal("Alterado", produto.Nome);
        }
        

        [Fact(DisplayName = "AlterarNome deve lançar DomainException se o Nome for inválido")]
        [Trait("Categoria", "Catalogo.Produto")]
        public void Produto_AlterarNome_DeveLancarDomainExceptionSeNomeForInvalido()
        {
            var produto = CriarProdutoValido();
            
            var stringVaziaException = Assert.Throws<DomainException>(() => produto.AlterarNome(""));
            var stringNullException = Assert.Throws<DomainException>(() => produto.AlterarNome(null));
            
            Assert.Equal("O campo Nome é obrigatório", stringVaziaException.Message);
            Assert.Equal("O campo Nome é obrigatório", stringNullException.Message);
        }

        [Fact(DisplayName = "Deve alterar o Valor do produto se o Valor for válido")]
        [Trait("Categoria", "Catalogo.Produto")]
        public void Produto_AlterarNome_DeveAlterarValorDoProduto()
        {
            var produto = CriarProdutoValido();

            produto.AlterarValor(999);

            Assert.Equal(999, produto.Valor);
        }

        [Fact(DisplayName = "AlterarValor deve lançar DomainException se o Valor for negativo")]
        [Trait("Categoria", "Catalogo.Produto")]
        public void Produto_AlterarValor_DeveLancarDomainExceptionSeValorForNegativo()
        {
            var produto = CriarProdutoValido();
            
            var exception = Assert.Throws<DomainException>(() => produto.AlterarValor(-1));
            
            Assert.Equal("O Valor não pode ser negativo", exception.Message);
        }

        [Fact(DisplayName = "Deve alterar o Estoque para a quantidade fornecida")]
        [Trait("Categoria", "Catalogo.Produto")]
        public void Produto_AlterarEstoque_DeveAlterarEstoqueParaQuantidadeFornecida()
        {
            var produto = CriarProdutoValido();

            produto.AlterarEstoque(999);

            Assert.Equal(999, produto.Estoque);
        }

        private Produto CriarProdutoValido(string nome = "Nome do Produto", int estoque = 10, decimal valor = 100)
        {
            return new Produto(nome, estoque, valor);
        }
    }
}