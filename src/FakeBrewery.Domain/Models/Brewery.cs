using System;
using System.Collections.Generic;

namespace FakeBrewery.Domain.Models
{
    public class Brewery
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Beer> Beers { get; set; }
    }
}