using System;
using System.Threading.Tasks;
using Catalogo.Core.Interfaces;
using Catalogo.Domain.Models;

namespace Catalogo.Domain.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task Inserir(Produto produto);
        Task<Produto> ObterPeloId(Guid produtoId);
        Task<bool> ExisteProdutoComId(Guid produtoId);
        Task Atualizar(Produto produto);
        Task Deletar(Guid produtoId);
    }
}