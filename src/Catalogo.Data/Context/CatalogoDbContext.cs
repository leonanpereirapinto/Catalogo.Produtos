using Catalogo.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Data.Context
{
    public class CatalogoDbContext : DbContext
    {
        public CatalogoDbContext(DbContextOptions<CatalogoDbContext> options) : base(options)
        {
            
        }
        
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var produtos = new Produto[]
            {
                new ("Produto de teste 1", 10, 100),
                new ("Produto de teste 2", 20, 200),
                new ("Produto de teste 3", 30, 300),
                new ("Produto de teste 4", 40, 400),
                new ("Produto de teste 5", 50, 500)
            };

            modelBuilder.Entity<Produto>().HasData(produtos);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}