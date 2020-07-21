using System.Collections.Generic;
using System.Linq;

namespace FakeBrewery.Application.Dtos
{
    public class Estimate
    {
        public double Discount { get; set; }
        public int TotalQuantity => Items.Sum(i => i.Quantity);
        public double TotalPriceWithoutVatAndDiscount => Items.Sum(i => i.TotalPriceWithoutVat);
        public double TotalPriceWithVatAndDiscount => TotalPriceWithoutVatAndDiscount * 1.21 * (1 - Discount);
        public ICollection<EstimateItem> Items { get; set; } = new List<EstimateItem>();
    }
}