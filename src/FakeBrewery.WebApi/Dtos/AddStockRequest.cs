using System;

namespace FakeBrewery.WebApi.Dtos
{
    public class AddStockRequest
    {
        public int Quantity { get; set; }
        public Guid WholesalerId { get; set; }
        public Guid BeerId { get; set; }
    }
}