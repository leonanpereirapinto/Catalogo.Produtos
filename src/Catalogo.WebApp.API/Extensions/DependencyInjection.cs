using Catalogo.Data.Context;
using Catalogo.Data.Repositories;
using Catalogo.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using WebApp.API.Interfaces;
using WebApp.API.Services;

namespace WebApp.API.Extensions
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IProdutoAppService, ProdutoAppService>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();

            services.AddScoped<CatalogoDbContext>();
        }
    }
}
