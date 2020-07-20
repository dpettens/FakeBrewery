using System;

namespace FakeBrewery.Domain.Models
{
    public class Stock
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }

        public Guid WholesalerId { get; set; }
        public Wholesaler Wholesaler { get; set; }

        public Guid BeerId { get; set; }
        public Beer Beer { get; set; }
    }
}