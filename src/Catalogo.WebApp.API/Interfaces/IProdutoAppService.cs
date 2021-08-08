using System;
using System.Threading.Tasks;
using WebApp.API.ViewModels;

namespace WebApp.API.Interfaces
{
    public interface IProdutoAppService
    {
        Task<bool> CriarProduto(CriarProdutoViewModel criarProdutoViewModel);
        Task<(bool sucesso, string mensagemErro)> AtualizarProduto(ProdutoViewModel produtoViewModel);
        Task<(bool sucesso, string mensagemErro)> RemoverProduto(Guid produtoId);
    }
}