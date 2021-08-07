using System;
using System.Threading.Tasks;
using Catalogo.Core.DomainObjects;

namespace Catalogo.Core.Interfaces
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        Task<bool> SaveChangesAsync();
    }
}