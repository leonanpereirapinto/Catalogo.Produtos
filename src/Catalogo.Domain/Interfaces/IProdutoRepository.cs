using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalogo.Core.Interfaces;
using Catalogo.Domain.Enums;
using Catalogo.Domain.Models;

namespace Catalogo.Domain.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<List<Produto>> ObterTodos(List<Guid> listaIdsConvertidos = null, List<string> listaNomes = null, OrdenarPor? ordenarPorEnum = null, Ordenacao? ordenacaoEnum = null);
        Task Inserir(Produto produto);
        Task<bool> ExisteProdutoComId(Guid produtoId);
        Task Atualizar(Produto produto);
        Task Deletar(Guid produtoId);
    }
}