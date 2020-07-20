using System.Linq;
using AutoMapper;
using FakeBrewery.Domain.Models;
using FakeBrewery.WebApi.Dtos;

namespace FakeBrewery.WebApi.Mappers
{
    public class BeerResponseProfile : Profile
    {
        public BeerResponseProfile()
        {
            CreateMap<Beer, BeerResponse>()
                .ForMember(
                    dest => dest.Wholesalers,
                    opts => opts.MapFrom(src => src.Stocks.Select(s => new BeerResponse.WholesalerResponse()
                    {
                        Id = s.Wholesaler.Id,
                        Name = s.Wholesaler.Name,
                        Quantity = s.Quantity
                    }))
                );

            CreateMap<Brewery, BeerResponse.BreweryResponse>();
        }
    }
}