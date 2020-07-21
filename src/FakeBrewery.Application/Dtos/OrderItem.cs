using System;

namespace FakeBrewery.Application.Dtos
{
    public class OrderItem
    {
        public Guid BeerId { get; set; }
        public int Quantity { get; set; }
    }
}