using System;
using System.Collections.Generic;

namespace FakeBrewery.Application.Dtos
{
    public class Order
    {
        public Guid WholesalerId { get; set; }
        public IEnumerable<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}