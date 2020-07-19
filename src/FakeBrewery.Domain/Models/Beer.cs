using System;

namespace FakeBrewery.Domain.Models
{
    public class Beer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Strength { get; set; }
        public double PriceWithoutVat { get; set; }
        public double PriceWithVat => PriceWithoutVat * 1.21;

        public Guid BreweryId { get; set; }
        public Brewery Brewery { get; set; }
    }
}
