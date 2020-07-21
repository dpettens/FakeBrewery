using System;

namespace FakeBrewery.WebApi.ApiModels
{
    public class AddBeerRequest
    {
        public string Name { get; set; }
        public double Strength { get; set; }
        public double PriceWithoutVat { get; set; }
        public Guid BreweryId { get; set; }
    }
}