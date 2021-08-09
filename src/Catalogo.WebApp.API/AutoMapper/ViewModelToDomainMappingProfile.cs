using AutoMapper;
using Catalogo.Domain.Models;
using WebApp.API.ViewModels;

namespace WebApp.API.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ProdutoViewModel, Produto>()
                .ConstructUsing(p => new Produto(p.Nome, p.Estoque.Value, p.Valor.Value)
                {
                    Id = p.Id
                });

            CreateMap<CriarProdutoViewModel, Produto>()
                .ConstructUsing(p => new Produto(p.Nome, p.Estoque, p.Valor));
        }
    }
}