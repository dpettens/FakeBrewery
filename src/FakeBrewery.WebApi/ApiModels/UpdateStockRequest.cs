using System;

namespace FakeBrewery.WebApi.ApiModels
{
    public class UpdateStockRequest
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
    }
}