using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalogo.Data.Context;
using Catalogo.Domain.Enums;
using Catalogo.Domain.Interfaces;
using Catalogo.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Data.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly CatalogoDbContext _context;

        public ProdutoRepository(CatalogoDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<Produto>> ObterTodos(List<Guid> listaIdsConvertidos = null, List<string> listaNomes = null, 
            OrdenarPor? ordenarPorEnum = null, Ordenacao? ordenacaoEnum = null)
        {
            var query = _context.Produtos.AsNoTracking();
            
            if (ordenarPorEnum.HasValue && ordenacaoEnum.HasValue)
            {
                query = (ordenarPorEnum.Value, ordenacaoEnum.Value) switch
                {
                    (OrdenarPor.Nome, Ordenacao.Asc) => query.OrderBy(p => p.Nome),
                    (OrdenarPor.Nome, Ordenacao.Desc) => query.OrderByDescending(p => p.Nome),
                    (OrdenarPor.Estoque, Ordenacao.Asc) => query.OrderBy(p => p.Estoque),
                    (OrdenarPor.Estoque, Ordenacao.Desc) => query.OrderByDescending(p => p.Estoque),
                    (OrdenarPor.Valor, Ordenacao.Asc) => query.OrderBy(p => p.Valor),
                    (OrdenarPor.Valor, Ordenacao.Desc) => query.OrderByDescending(p => p.Valor),
                };
            }

            if (listaIdsConvertidos?.Any() == true)
            {
                query = query.Where(p => listaIdsConvertidos.Contains(p.Id.Value));
            }

            if (listaNomes?.Any() == true)
            {
                query = query.Where(p => listaNomes.Contains(p.Nome));
            }

            return await query.ToListAsync();
        }

        public async Task<Produto> ObterPeloId(Guid produtoId)
        {
            return await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == produtoId);
        }

        public async Task Inserir(Produto produto)
        {
            await _context.Produtos.AddAsync(produto);
        }

        public async Task<bool> ExisteProdutoComId(Guid produtoId)
        {
            return await _context.Produtos.AsNoTracking().AnyAsync(p => p.Id == produtoId);
        }

        public void Atualizar(Produto produto)
        {
            _context.Produtos.Update(produto);
        }

        public void Deletar(Produto produto)
        {
            _context.Produtos.Remove(produto);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
