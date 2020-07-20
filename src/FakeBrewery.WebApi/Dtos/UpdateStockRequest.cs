using System;

namespace FakeBrewery.WebApi.Dtos
{
    public class UpdateStockRequest
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
    }
}