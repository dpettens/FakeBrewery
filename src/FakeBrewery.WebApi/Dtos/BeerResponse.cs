using System;
using System.Collections.Generic;

namespace FakeBrewery.WebApi.Dtos
{
    public class BeerResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Strength { get; set; }
        public double PriceWithoutVat { get; set; }
        public double PriceWithVat => PriceWithoutVat * 1.21;

        public BreweryResponse Brewery { get; set; }
        public IEnumerable<WholesalerResponse> Wholesalers { get; set; }

        public class BreweryResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class WholesalerResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
        }
    }
}