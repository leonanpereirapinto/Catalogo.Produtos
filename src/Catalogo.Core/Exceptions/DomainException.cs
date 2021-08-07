using System;

namespace Catalogo.Core.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string mensagem) : base(mensagem)
        {
        }
    }
}