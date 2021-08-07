using Catalogo.Core.Exceptions;

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
            ValidarValor(valor);

            Nome = nome;
            Estoque = estoque;
            Valor = valor;
        }

        private void ValidarNome(string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                throw new DomainException("O campo Nome é obrigatório");
            }
        }

        private void ValidarValor(decimal valor)
        {
            if (valor < 0)
            {
                throw new DomainException("O Valor não pode ser negativo");
            }
        }
    }
}