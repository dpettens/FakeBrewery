using System;
using FakeBrewery.Domain.Models;
using FakeBrewery.Domain.Specifications;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FakeBrewery.Domain.Tests
{
    [TestClass]
    public class BeerSpecificationsTests
    {
        #region HasValidId

        [TestMethod]
        public void HasValidId_EmptyId_ReturnsFalse()
        {
            // Arrange
            var beer = new Beer { Id = Guid.Empty };

            var spec = BeerSpecifications.HasValidId;

            // Act
            var isValid = spec.IsSatisfiedBy(beer);

            // Assert
            isValid.Should().BeFalse();
        }

        [TestMethod]
        public void HasValidId_ValidId_ReturnsTrue()
        {
            // Arrange
            var beer = new Beer { Id = Guid.NewGuid() };

            var spec = BeerSpecifications.HasValidId;

            // Act
            var isValid = spec.IsSatisfiedBy(beer);

            // Assert
            isValid.Should().BeTrue();
        }

        #endregion

        #region HasValidName

        [TestMethod]
        public void HasValidName_NullAndEmptyAndWhiteSpaceName_ReturnsFalse()
        {
            // Arrange
            var beerNullName = new Beer { Name = null };
            var beerEmptyName = new Beer { Name = string.Empty };
            var beerWhiteSpaceName = new Beer { Name = "" };

            var spec = BeerSpecifications.HasValidName;

            // Act
            var isValidNullName = spec.IsSatisfiedBy(beerNullName);
            var isValidEmptyName = spec.IsSatisfiedBy(beerEmptyName);
            var isValidWhiteSpaceName = spec.IsSatisfiedBy(beerWhiteSpaceName);

            // Assert
            isValidNullName.Should().BeFalse();
            isValidEmptyName.Should().BeFalse();
            isValidWhiteSpaceName.Should().BeFalse();
        }

        [TestMethod]
        public void HasValidName_ValidName_ReturnsTrue()
        {
            // Arrange
            var beer = new Beer { Name = "Leffe Brune" };

            var spec = BeerSpecifications.HasValidName;

            // Act
            var isValid = spec.IsSatisfiedBy(beer);

            // Assert
            isValid.Should().BeTrue();
        }

        #endregion

        #region HasValidPrice

        [TestMethod]
        public void HasValidPrice_NegativeAndFreePrice_ReturnsFalse()
        {
            // Arrange
            var beerNegativePrice = new Beer() { PriceWithoutVat = -25 };
            var beerFreePrice = new Beer() { PriceWithoutVat = 0 };

            var spec = BeerSpecifications.HasValidPrice;

            // Act
            var isValidNegativePrice = spec.IsSatisfiedBy(beerNegativePrice);
            var isValidFreePrice = spec.IsSatisfiedBy(beerFreePrice);

            // Assert
            isValidNegativePrice.Should().BeFalse();
            isValidFreePrice.Should().BeFalse();
        }

        [TestMethod]
        public void HasValidPrice_PositivePrice_ReturnsTrue()
        {
            // Arrange
            var beer = new Beer { PriceWithoutVat = 5 };

            var spec = BeerSpecifications.HasValidPrice;

            // Act
            var isValid = spec.IsSatisfiedBy(beer);

            // Assert
            isValid.Should().BeTrue();
        }

        #endregion

        #region HasValidStrength

        [TestMethod]
        public void HasValidStrength_NegativeStrength_ReturnsFalse()
        {
            // Arrange
            var beerNegativeStrength = new Beer() { Strength = -5 };
            
            var spec = BeerSpecifications.HasValidStrength;

            // Act
            var isValidNegativeStrength = spec.IsSatisfiedBy(beerNegativeStrength);

            // Assert
            isValidNegativeStrength.Should().BeFalse();
        }

        [TestMethod]
        public void HasValidStrength_PositiveAndZeroStrength_ReturnsTrue()
        {
            // Arrange
            var beerPositiveStrength = new Beer { Strength = 5 };
            var beerZeroStrength = new Beer() { Strength = 0 };

            var spec = BeerSpecifications.HasValidStrength;

            // Act
            var isValidPositiveStrength = spec.IsSatisfiedBy(beerPositiveStrength);
            var isValidZeroStrength = spec.IsSatisfiedBy(beerZeroStrength);

            // Assert
            isValidPositiveStrength.Should().BeTrue();
            isValidZeroStrength.Should().BeTrue();
        }

        #endregion

        #region HasValidBreweryId

        [TestMethod]
        public void HasValidBreweryId_EmptyId_ReturnsFalse()
        {
            // Arrange
            var beer = new Beer { BreweryId = Guid.Empty };

            var spec = BeerSpecifications.HasValidBreweryId;

            // Act
            var isValid = spec.IsSatisfiedBy(beer);

            // Assert
            isValid.Should().BeFalse();
        }

        [TestMethod]
        public void HasValidBreweryId_ValidId_ReturnsTrue()
        {
            // Arrange
            var beer = new Beer { BreweryId = Guid.NewGuid() };

            var spec = BeerSpecifications.HasValidBreweryId;

            // Act
            var isValid = spec.IsSatisfiedBy(beer);

            // Assert
            isValid.Should().BeTrue();
        }

        #endregion
    }
}