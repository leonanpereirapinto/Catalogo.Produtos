using System.Threading.Tasks;
using WebApp.API.Interfaces;
using WebApp.API.ViewModels;

namespace WebApp.API.Services
{
    public class ProdutoService : IProdutoService
    {
        public async Task<bool> CriarProduto(CriarProdutoViewModel criarProdutoViewModel)
        {
            return false;
        }
    }
}