using AutoMapper;
using Catalogo.Domain.Models;
using WebApp.API.ViewModels;

namespace WebApp.API.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Produto, ProdutoViewModel>();
        }
    }
}
