using System;
using System.Collections.Generic;
using FakeBrewery.Application.Dtos;
using FakeBrewery.Application.Specifications;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FakeBrewery.Application.Tests
{
    [TestClass]
    public class OrderSpecificationsTests
    {
        #region IsEmpty

        [TestMethod]
        public void IsEmpty_EmptyItems_ReturnsFalse()
        {
            // Arrange
            var order = new Order()
            {
                WholesalerId = Guid.NewGuid(),
                Items = new List<OrderItem>()
            };

            var spec = OrderSpecifications.IsEmpty;

            // Act
            var isEmpty = spec.IsSatisfiedBy(order);

            // Assert
            isEmpty.Should().BeTrue();
        }

        [TestMethod]
        public void IsEmpty_EmptyWholesalerId_ReturnsFalse()
        {
            // Arrange
            var order = new Order()
            {
                WholesalerId = Guid.Empty,
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = Guid.NewGuid(), Quantity = 20 }
                }
            };

            var spec = OrderSpecifications.IsEmpty;

            // Act
            var isEmpty = spec.IsSatisfiedBy(order);

            // Assert
            isEmpty.Should().BeTrue();
        }

        [TestMethod]
        public void IsEmpty_EmptyWholesalerIdAndItems_ReturnsFalse()
        {
            // Arrange
            var order = new Order()
            {
                WholesalerId = Guid.Empty,
                Items = new List<OrderItem>()
            };

            var spec = OrderSpecifications.IsEmpty;

            // Act
            var isEmpty = spec.IsSatisfiedBy(order);

            // Assert
            isEmpty.Should().BeTrue();
        }

        [TestMethod]
        public void IsEmpty_ValidOrder_ReturnsTrue()
        {
            // Arrange
            var order = new Order()
            {
                WholesalerId = Guid.NewGuid(),
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = Guid.NewGuid(), Quantity = 20 }
                }
            };

            var spec = OrderSpecifications.IsEmpty;

            // Act
            var isEmpty = spec.IsSatisfiedBy(order);

            // Assert
            isEmpty.Should().BeFalse();
        }

        #endregion

        #region IsAllItemsValid

        [TestMethod]
        public void IsAllItemsValid_NullItem_ReturnsFalse()
        {
            // Arrange
            var order = new Order()
            {
                WholesalerId = Guid.NewGuid(),
                Items = new List<OrderItem>
                {
                    null
                }
            };

            var spec = OrderSpecifications.IsAllItemsValid;

            // Act
            var isEmpty = spec.IsSatisfiedBy(order);

            // Assert
            isEmpty.Should().BeFalse();
        }

        [TestMethod]
        public void IsAllItemsValid_InvalidBeerIdItem_ReturnsFalse()
        {
            // Arrange
            var order = new Order()
            {
                WholesalerId = Guid.NewGuid(),
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = Guid.Empty, Quantity = 25 }
                }
            };

            var spec = OrderSpecifications.IsAllItemsValid;

            // Act
            var isEmpty = spec.IsSatisfiedBy(order);

            // Assert
            isEmpty.Should().BeFalse();
        }

        [TestMethod]
        public void IsAllItemsValid_InvalidQuantityItem_ReturnsFalse()
        {
            // Arrange
            var order = new Order()
            {
                WholesalerId = Guid.NewGuid(),
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = Guid.NewGuid(), Quantity = -25 }
                }
            };

            var spec = OrderSpecifications.IsAllItemsValid;

            // Act
            var isEmpty = spec.IsSatisfiedBy(order);

            // Assert
            isEmpty.Should().BeFalse();
        }

        [TestMethod]
        public void IsAllItemsValid_InvalidBeerIdAndQuantityItem_ReturnsFalse()
        {
            // Arrange
            var order = new Order()
            {
                WholesalerId = Guid.NewGuid(),
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = Guid.Empty, Quantity = -25 }
                }
            };

            var spec = OrderSpecifications.IsAllItemsValid;

            // Act
            var isEmpty = spec.IsSatisfiedBy(order);

            // Assert
            isEmpty.Should().BeFalse();
        }

        [TestMethod]
        public void IsAllItemsValid_OneValidAndOneInvalidItems_ReturnsFalse()
        {
            // Arrange
            var order = new Order()
            {
                WholesalerId = Guid.NewGuid(),
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = Guid.NewGuid(), Quantity = -25 },
                    new OrderItem { BeerId = Guid.NewGuid(), Quantity = 20 }
                }
            };

            var spec = OrderSpecifications.IsAllItemsValid;

            // Act
            var isEmpty = spec.IsSatisfiedBy(order);

            // Assert
            isEmpty.Should().BeFalse();
        }

        [TestMethod]
        public void IsAllItemsValid_ValidItems_ReturnsTrue()
        {
            // Arrange
            var order = new Order()
            {
                WholesalerId = Guid.NewGuid(),
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = Guid.NewGuid(), Quantity = 25 },
                    new OrderItem { BeerId = Guid.NewGuid(), Quantity = 20 }
                }
            };

            var spec = OrderSpecifications.IsAllItemsValid;

            // Act
            var isEmpty = spec.IsSatisfiedBy(order);

            // Assert
            isEmpty.Should().BeTrue();
        }

        #endregion

        #region HasDuplicateItems

        [TestMethod]
        public void HasDuplicateItems_TwoDuplicates_ReturnsTrue()
        {
            // Arrange
            var order = new Order()
            {
                WholesalerId = Guid.NewGuid(),
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = new Guid("750D943C-1005-447D-B20E-696BBA111119"), Quantity = 20 },
                    new OrderItem { BeerId = new Guid("750D943C-1005-447D-B20E-696BBA111119"), Quantity = 10 }
                }
            };

            var spec = OrderSpecifications.HasDuplicateItems;

            // Act
            var isEmpty = spec.IsSatisfiedBy(order);

            // Assert
            isEmpty.Should().BeTrue();
        }

        [TestMethod]
        public void HasDuplicateItems_NoDuplicate_ReturnsFalse()
        {
            // Arrange
            var order = new Order()
            {
                WholesalerId = Guid.NewGuid(),
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = new Guid("750D943C-1005-447D-B20E-696BBA111119"), Quantity = 20 },
                    new OrderItem { BeerId = new Guid("6EF61398-C3F1-4BF0-91B7-F36129E01999"), Quantity = 10 }
                }
            };

            var spec = OrderSpecifications.HasDuplicateItems;

            // Act
            var isEmpty = spec.IsSatisfiedBy(order);

            // Assert
            isEmpty.Should().BeFalse();
        }

        #endregion
    }
}
