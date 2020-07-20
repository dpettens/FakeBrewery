using AutoMapper;
using FakeBrewery.Domain.Models;
using FakeBrewery.WebApi.Dtos;

namespace FakeBrewery.WebApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddBeerRequest, Beer>();
            CreateMap<AddStockRequest, Stock>();
        }
    }
}