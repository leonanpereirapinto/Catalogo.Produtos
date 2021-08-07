using Catalogo.Core.DomainObjects;
using Catalogo.Core.Exceptions;

namespace Catalogo.Domain.Models
{
    public class Produto : Entity, IAggregateRoot
    {
        public string Nome { get; private set; }

        public int Estoque { get; private set; }

        public decimal Valor { get; private set; }
        
        protected Produto() { } // EF Core

        public Produto(string nome, int estoque, decimal valor)
        {
            ValidarNome(nome);
            ValidarValor(valor);

            Nome = nome;
            Estoque = estoque;
            Valor = valor;
        }

        public void AlterarNome(string nome)
        {
            ValidarNome(nome);

            Nome = nome;
        }

        public void AlterarValor(decimal valor)
        {
            ValidarValor(valor);

            Valor = valor;
        }

        public void AlterarEstoque(int estoque)
        {
            Estoque = estoque;
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
                throw new DomainException("O Valor do produto não pode ser negativo");
            }
        }
    }
}