using System;

namespace Catalogo.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string mensagem) : base(mensagem)
        {
        }
    }
}