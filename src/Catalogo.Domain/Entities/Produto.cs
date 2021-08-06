namespace Catalogo.Domain.Entities
{
    public class Produto
    {
        public string Nome { get; private set; }

        public int Estoque { get; private set; }

        public decimal Valor { get; private set; }
        
        protected Produto() { }

        public Produto(string nome, int estoque, decimal valor)
        {
            ValidarNome(nome);
            ValidarEstoque(estoque);
            ValidarValor(valor);

            Nome = nome;
            Estoque = estoque;
            Valor = valor;
        }

        private void ValidarNome(string nome)
        {
        }

        private void ValidarValor(decimal valor)
        {
        }

        private void ValidarEstoque(int estoque)
        {
        }
    }
}