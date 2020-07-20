using AutoMapper;
using FakeBrewery.Domain.Models;
using FakeBrewery.WebApi.Dtos;

namespace FakeBrewery.WebApi.Mappers
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<AddBeerRequest, Beer>();
            CreateMap<AddStockRequest, Stock>();
            CreateMap<UpdateStockRequest, Stock>();
        }
    }
}