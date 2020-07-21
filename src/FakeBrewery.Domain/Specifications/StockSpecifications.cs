using System;
using FakeBrewery.Domain.Models;
using NSpecifications;

namespace FakeBrewery.Domain.Specifications
{
    public static class StockSpecifications
    {
        public static ASpec<Stock> HasValidId => new Spec<Stock>(s => s.Id != Guid.Empty);
        public static ASpec<Stock> HasValidQuantity => new Spec<Stock>(s => s.Quantity >= 0);
        public static ASpec<Stock> HasValidBeerId => new Spec<Stock>(s => s.BeerId != Guid.Empty);
        public static ASpec<Stock> HasValidWholesalerId => new Spec<Stock>(s => s.WholesalerId != Guid.Empty);
    }
}