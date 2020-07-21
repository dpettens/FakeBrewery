using System;

namespace FakeBrewery.Application.Dtos
{
    public class EstimateItem
    {
        public Guid BeerId { get; set; }
        public string BeerName { get; set; }
        public int Quantity { get; set; }
        public double UnitPriceWithoutVat { get; set; }
        public double TotalPriceWithoutVat => UnitPriceWithoutVat * Quantity;
    }
}