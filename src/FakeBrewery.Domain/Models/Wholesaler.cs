using System;
using System.Collections.Generic;

namespace FakeBrewery.Domain.Models
{
    public class Wholesaler
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    }
}