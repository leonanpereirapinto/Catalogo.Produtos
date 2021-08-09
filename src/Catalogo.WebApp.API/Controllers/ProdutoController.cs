using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Interfaces;
using WebApp.API.ViewModels;

namespace WebApp.API.Controllers
{
    [Route("produtos")]
    public class ProdutoController : ApiController
    {
        private IProdutoAppService _produtoAppService;

        public ProdutoController(IProdutoAppService produtoAppService)
        {
            _produtoAppService = produtoAppService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos(ObterTodosViewModel obterTodosViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse();
            }
            
            var (viewModels, mensagemErro) = await _produtoAppService.ObterTodos(obterTodosViewModel);

            if (!string.IsNullOrEmpty(mensagemErro))
            {
                return BadRequest(mensagemErro);
            }

            return Ok(viewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(CriarProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse();
            }

            var resultado = await _produtoAppService.CriarProduto(produtoViewModel);

            if (!resultado)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Atualizar(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse();
            }

            if (produtoViewModel.Id != id)
            {
                return BadRequest("ID da requisição é diferente do ID no corpo da requisição");
            }

            var (sucesso, mensagemErro) = await _produtoAppService.AtualizarProduto(produtoViewModel);

            if (sucesso) return Ok();

            if (string.IsNullOrEmpty(mensagemErro))
            {
                return BadRequest();
            }

            return BadRequest(mensagemErro);
        }
    }
}