using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalogo.Domain.Enums;
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

        public async Task<(List<ProdutoViewModel> produtos, string mensagemErro)> ObterTodos(ObterTodosViewModel obterTodosViewModel)
        {
            List<Guid> listaIdsConvertidos = null;

            if (obterTodosViewModel == null)
            {
                return (_mapper.Map<List<ProdutoViewModel>>(await _produtoRepository.ObterTodos()), null);
            }

            var listaDeIds = obterTodosViewModel.Id?.Split(",");

            if (listaDeIds?.Any() == true)
            {
                if (listaDeIds.Any(id => !Guid.TryParse(id, out _)))
                {
                    return (null, "Id com formato inválido fornecido");
                }

                listaIdsConvertidos = listaDeIds.Where(id => !string.IsNullOrEmpty(id)).Select(Guid.Parse).ToList();
            }

            var listaNomes = obterTodosViewModel.Nome?.Split(",").ToList();

            OrdenarPor? ordenarPorEnum = null;
            Ordenacao? ordenacaoEnum = null;

            if (!string.IsNullOrEmpty(obterTodosViewModel.OrdenarPor))
            {
                var ordenarPor = obterTodosViewModel.OrdenarPor.Split(":");

                if (ordenarPor.Length != 2)
                {
                    return (null, "OrdenarPor possui um formato inválido");
                }

                if (!Enum.TryParse<OrdenarPor>(ordenarPor[0], true, out var ordenarPorParsed))
                {
                    return (null, "A primeira parte do campo OrdenarPor possui um formato inválido");
                }

                ordenarPorEnum = ordenarPorParsed;

                if (!Enum.TryParse<Ordenacao>(ordenarPor[1], true, out var ordenacao))
                {
                    return (null, "A segunda parte do campo OrdenarPor possui um formato inválido");
                }

                ordenacaoEnum = ordenacao;
            }

            var produtos = await _produtoRepository.ObterTodos(listaIdsConvertidos, listaNomes, ordenarPorEnum, ordenacaoEnum);

            var produtoViewModels = _mapper.Map<List<ProdutoViewModel>>(produtos);

            return (produtoViewModels, null);
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

            if (!string.IsNullOrEmpty(produtoViewModel.Nome))
            {
                produtoExistente.AlterarNome(produtoViewModel.Nome);
            }

            if (produtoViewModel.Estoque.HasValue)
            {
                produtoExistente.AlterarEstoque(produtoViewModel.Estoque.Value);
            }

            if (produtoViewModel.Valor.HasValue)
            {
                produtoExistente.AlterarValor(produtoViewModel.Valor.Value);
            }

            _produtoRepository.Atualizar(produtoExistente);

            return (await _produtoRepository.SaveChangesAsync(), null);
        }

        public async Task<(bool sucesso, string mensagemErro)> RemoverProduto(Guid produtoId)
        {
            var produtoExistente = await _produtoRepository.ObterPeloId(produtoId);

            if (produtoExistente == null)
            {
                return (false, $"Produto com o Id {produtoId} não foi encontrado");
            }

            _produtoRepository.Deletar(produtoExistente);

            return (await _produtoRepository.SaveChangesAsync(), null);
        }
    }
}