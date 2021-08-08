using System.Threading.Tasks;
using AutoMapper;
using Catalogo.Domain.Interfaces;
using Catalogo.Domain.Models;
using WebApp.API.Interfaces;
using WebApp.API.ViewModels;

namespace WebApp.API.Services
{
    public class ProdutoAppService : IProdutoAppService
    {
        private readonly IMapper _mapper;
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoAppService(IMapper mapper, IProdutoRepository produtoRepository)
        {
            _mapper = mapper;
            _produtoRepository = produtoRepository;
        }

        public async Task<bool> CriarProduto(CriarProdutoViewModel criarProdutoViewModel)
        {
            var produto = _mapper.Map<Produto>(criarProdutoViewModel);

            await _produtoRepository.Inserir(produto);

            return await _produtoRepository.SaveChangesAsync();
        }
        
        public async Task<(bool sucesso, string mensagemErro)> AtualizarProduto(ProdutoViewModel produtoViewModel)
        {
            if (!produtoViewModel.Id.HasValue)
            {
                return (false, "Id do produto é obrigatório");
            }

            var produtoId = produtoViewModel.Id.Value;
            var produtoExistente = await _produtoRepository.ObterPeloId(produtoId);

            if (produtoExistente == null)
            {
                return (false, $"Produto com o Id {produtoId} não foi encontrado");
            }
            
            
            var produto = _mapper.Map<Produto>(produtoViewModel);

            await _produtoRepository.Atualizar(produto);

            return (await _produtoRepository.SaveChangesAsync(), null);
        }
    }
}