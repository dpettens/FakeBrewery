using System;
using FakeBrewery.Domain.Models;
using NSpecifications;

namespace FakeBrewery.Domain.Specifications
{
    public static class BeerSpecifications
    {
        public static ASpec<Beer> HasValidId => new Spec<Beer>(b => b.Id != Guid.Empty);
        public static ASpec<Beer> HasValidName => new Spec<Beer>(b => !string.IsNullOrWhiteSpace(b.Name));
        public static ASpec<Beer> HasValidStrength => new Spec<Beer>(b => b.Strength >= 0);
        public static ASpec<Beer> HasValidPrice => new Spec<Beer>(b => b.PriceWithoutVat > 0);
        public static ASpec<Beer> HasValidBreweryId => new Spec<Beer>(b => b.BreweryId != Guid.Empty);
    }
}