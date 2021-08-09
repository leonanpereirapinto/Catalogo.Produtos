using System;
using System.Collections.Generic;
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

        public async Task<List<Produto>> ObterTodos(List<Guid> listaIdsConvertidos = null, List<string> listaNomes = null, OrdenarPor? ordenarPorEnum = null,
            Ordenacao? ordenacaoEnum = null)
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
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

        public Task Atualizar(Produto produto)
        {
            throw new NotImplementedException();
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
