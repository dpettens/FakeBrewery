using System;
using System.Linq;
using FakeBrewery.Application.Dtos;
using NSpecifications;

namespace FakeBrewery.Application.Specifications
{
    public static class OrderSpecifications
    {
        public static ASpec<Order> IsEmpty => new Spec<Order>(o => o.WholesalerId == Guid.Empty || !o.Items.Any());
        public static ASpec<Order> IsAllItemsValid => new Spec<Order>(o => o.Items.All(i => i != null && i.Quantity > 0 && i.BeerId != Guid.Empty));
        public static ASpec<Order> HasDuplicateItems => new Spec<Order>(o => o.Items.Count() != o.Items.Select(i => i.BeerId).Distinct().Count());
    }
}