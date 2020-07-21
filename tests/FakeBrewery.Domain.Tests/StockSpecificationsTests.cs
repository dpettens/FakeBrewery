using System;
using FakeBrewery.Domain.Models;
using FakeBrewery.Domain.Specifications;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FakeBrewery.Domain.Tests
{
    [TestClass]
    public class StockSpecificationsTests
    {
        #region HasValidId

        [TestMethod]
        public void HasValidId_EmptyId_ReturnsFalse()
        {
            // Arrange
            var stock = new Stock { Id = Guid.Empty };

            var spec = StockSpecifications.HasValidId;

            // Act
            var isValid = spec.IsSatisfiedBy(stock);

            // Assert
            isValid.Should().BeFalse();
        }

        [TestMethod]
        public void HasValidId_ValidId_ReturnsTrue()
        {
            // Arrange
            var stock = new Stock { Id = Guid.NewGuid() };

            var spec = StockSpecifications.HasValidId;

            // Act
            var isValid = spec.IsSatisfiedBy(stock);

            // Assert
            isValid.Should().BeTrue();
        }

        #endregion

        #region HasValidQuantity

        [TestMethod]
        public void HasValidQuantity_NegativeQuantity_ReturnsFalse()
        {
            // Arrange
            var stock = new Stock() { Quantity = -25 };

            var spec = StockSpecifications.HasValidQuantity;

            // Act
            var isValid = spec.IsSatisfiedBy(stock);

            // Assert
            isValid.Should().BeFalse();
        }

        [TestMethod]
        public void HasValidPrice_PositiveAndZeroQuantity_ReturnsTrue()
        {
            // Arrange
            var stockPositiveQuantity = new Stock() { Quantity = 25 };
            var stockZeroQuantity = new Stock() { Quantity = 0 };

            var spec = StockSpecifications.HasValidQuantity;

            // Act
            var isValidPositiveQuantity = spec.IsSatisfiedBy(stockPositiveQuantity);
            var isValidZeroQuantity = spec.IsSatisfiedBy(stockZeroQuantity);

            // Assert
            isValidPositiveQuantity.Should().BeTrue();
            isValidZeroQuantity.Should().BeTrue();
        }

        #endregion

        #region HasValidBeerId

        [TestMethod]
        public void HasValidBeerId_EmptyId_ReturnsFalse()
        {
            // Arrange
            var stock = new Stock { BeerId = Guid.Empty };

            var spec = StockSpecifications.HasValidBeerId;

            // Act
            var isValid = spec.IsSatisfiedBy(stock);

            // Assert
            isValid.Should().BeFalse();
        }

        [TestMethod]
        public void HasValidBeerId_ValidId_ReturnsTrue()
        {
            // Arrange
            var stock = new Stock { BeerId = Guid.NewGuid() };

            var spec = StockSpecifications.HasValidBeerId;

            // Act
            var isValid = spec.IsSatisfiedBy(stock);

            // Assert
            isValid.Should().BeTrue();
        }

        #endregion

        #region HasValidWholesalerId

        [TestMethod]
        public void HasValidWholesalerId_EmptyId_ReturnsFalse()
        {
            // Arrange
            var stock = new Stock { WholesalerId = Guid.Empty };

            var spec = StockSpecifications.HasValidWholesalerId;

            // Act
            var isValid = spec.IsSatisfiedBy(stock);

            // Assert
            isValid.Should().BeFalse();
        }

        [TestMethod]
        public void HasValidWholesalerId_ValidId_ReturnsTrue()
        {
            // Arrange
            var stock = new Stock { WholesalerId = Guid.NewGuid() };

            var spec = StockSpecifications.HasValidWholesalerId;

            // Act
            var isValid = spec.IsSatisfiedBy(stock);

            // Assert
            isValid.Should().BeTrue();
        }

        #endregion
    }
}