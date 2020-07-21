using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeBrewery.Application.Dtos;
using FakeBrewery.Application.Services;
using FakeBrewery.Domain.Models;
using FakeBrewery.Infra.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;

namespace FakeBrewery.Application.Tests
{
    [TestClass]
    public class WholesalerServiceTests
    {
        #region CalcualteEstimate

        [TestMethod]
        public async Task CalculateEstimate_EmptyWholesalerId_ReturnsFailureResultWithValidationCode()
        {
            // Arrange
            var order = new Order()
            {
                WholesalerId = Guid.Empty,
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = Guid.NewGuid(), Quantity = 10 }
                }
            };

            var breweryContextMock = new Mock<BreweryContext>();
            var wholesalerService = new WholesalerService(breweryContextMock.Object);

            // Act
            var result = await wholesalerService.CalculateEstimate(order);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.ErrorCode.Should().Be(ResultErrorCode.Validation);
        }

        [TestMethod]
        public async Task CalculateEstimate_EmptyItems_ReturnsFailureResultWithValidationCode()
        {
            // Arrange
            var order = new Order()
            {
                WholesalerId = Guid.NewGuid(),
                Items = new List<OrderItem>()
            };

            var breweryContextMock = new Mock<BreweryContext>();
            var wholesalerService = new WholesalerService(breweryContextMock.Object);

            // Act
            var result = await wholesalerService.CalculateEstimate(order);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.ErrorCode.Should().Be(ResultErrorCode.Validation);
        }

        [TestMethod]
        public async Task CalculateEstimate_InvalidItem_ReturnsFailureResultWithValidationCode()
        {
            // Arrange
            var order = new Order()
            {
                WholesalerId = Guid.NewGuid(),
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = Guid.Empty, Quantity = -10 }
                }
            };

            var breweryContextMock = new Mock<BreweryContext>();
            var wholesalerService = new WholesalerService(breweryContextMock.Object);

            // Act
            var result = await wholesalerService.CalculateEstimate(order);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.ErrorCode.Should().Be(ResultErrorCode.Validation);
        }

        [TestMethod]
        public async Task CalculateEstimate_DuplicateItems_ReturnsFailureResultWithValidationCode()
        {
            // Arrange
            var order = new Order()
            {
                WholesalerId = Guid.NewGuid(),
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = new Guid("253F30AC-ACF7-4577-AD8F-F7252B0979C8"), Quantity = 5 },
                    new OrderItem { BeerId = new Guid("253F30AC-ACF7-4577-AD8F-F7252B0979C8"), Quantity = 10 }
                }
            };

            var breweryContextMock = new Mock<BreweryContext>();
            var wholesalerService = new WholesalerService(breweryContextMock.Object);

            // Act
            var result = await wholesalerService.CalculateEstimate(order);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.ErrorCode.Should().Be(ResultErrorCode.Validation);
        }

        [TestMethod]
        public async Task CalculateEstimate_InvalidWholesaler_ReturnsFailureResultWithNotFoundCode()
        {
            // Arrange
            var order = new Order()
            {
                WholesalerId = Guid.NewGuid(),
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = new Guid("253F30AC-ACF7-4577-AD8F-F7252B0979C8"), Quantity = 5 }
                }
            };

            var wholesalers = new List<Wholesaler>
            {
                new Wholesaler { Id = Guid.NewGuid(), Name = "Test Wholesaler" }
            };

            var breweryContextMock = new Mock<BreweryContext>();
            breweryContextMock.Setup(x => x.Wholesalers).ReturnsDbSet(wholesalers);

            var wholesalerService = new WholesalerService(breweryContextMock.Object);

            // Act
            var result = await wholesalerService.CalculateEstimate(order);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.ErrorCode.Should().Be(ResultErrorCode.NotFound);
        }

        [TestMethod]
        public async Task CalculateEstimate_NotSellBeer_ReturnsFailureResultWithNotFoundCode()
        {
            // Arrange
            var wholesalerId = new Guid("9D01DFC3-1227-427B-8E02-FCEC6522215B");

            var order = new Order()
            {
                WholesalerId = wholesalerId,
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = new Guid("253F30AC-ACF7-4577-AD8F-F7252B0979C8"), Quantity = 5 }
                }
            };

            var wholesalers = new List<Wholesaler>
            {
                new Wholesaler
                {
                    Id = wholesalerId, 
                    Name = "Test Wholesaler", 
                    Stocks = new List<Stock>
                    {
                        new Stock { Id = Guid.NewGuid(), BeerId = Guid.NewGuid(), WholesalerId = wholesalerId, Quantity = 10 }
                    }
                }
            };

            var breweryContextMock = new Mock<BreweryContext>();
            breweryContextMock.Setup(x => x.Wholesalers).ReturnsDbSet(wholesalers);

            var wholesalerService = new WholesalerService(breweryContextMock.Object);

            // Act
            var result = await wholesalerService.CalculateEstimate(order);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.ErrorCode.Should().Be(ResultErrorCode.NotFound);
        }

        [TestMethod]
        public async Task CalculateEstimate_NotEnoughBeers_ReturnsFailureResultWithBusinessCode()
        {
            // Arrange
            var wholesalerId = new Guid("9D01DFC3-1227-427B-8E02-FCEC6522215B");
            var beerId = new Guid("253F30AC-ACF7-4577-AD8F-F7252B0979C8");

            var order = new Order()
            {
                WholesalerId = wholesalerId,
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = beerId, Quantity = 5 }
                }
            };

            var wholesalers = new List<Wholesaler>
            {
                new Wholesaler
                {
                    Id = wholesalerId,
                    Name = "Test Wholesaler",
                    Stocks = new List<Stock>
                    {
                        new Stock { Id = Guid.NewGuid(), BeerId = beerId, WholesalerId = wholesalerId, Quantity = 2 }
                    }
                }
            };

            var breweryContextMock = new Mock<BreweryContext>();
            breweryContextMock.Setup(x => x.Wholesalers).ReturnsDbSet(wholesalers);

            var wholesalerService = new WholesalerService(breweryContextMock.Object);

            // Act
            var result = await wholesalerService.CalculateEstimate(order);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.ErrorCode.Should().Be(ResultErrorCode.Business);
        }

        [TestMethod]
        public async Task CalculateEstimate_Buy7Beers_ReturnsSuccessResultWithNoDiscount()
        {
            // Arrange
            var wholesalerId = new Guid("9D01DFC3-1227-427B-8E02-FCEC6522215B");
            var leffeBruneBeerId = new Guid("253F30AC-ACF7-4577-AD8F-F7252B0979C8");
            var leffeTripleBeerId = new Guid("F781DCCB-E4D9-4CD6-9684-0FDFB7E136CA");

            var order = new Order()
            {
                WholesalerId = wholesalerId,
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = leffeBruneBeerId, Quantity = 4 },
                    new OrderItem { BeerId = leffeTripleBeerId, Quantity = 3 }
                }
            };

            var leffeBruneBeer = new Beer()
            {
                Id = leffeBruneBeerId,
                Name = "Leffe Brune",
                PriceWithoutVat = 3.5
            };

            var leffeTripleBeer = new Beer()
            {
                Id = leffeBruneBeerId,
                Name = "Leffe Triple",
                PriceWithoutVat = 5
            };

            var wholesalers = new List<Wholesaler>
            {
                new Wholesaler
                {
                    Id = wholesalerId,
                    Name = "Test Wholesaler",
                    Stocks = new List<Stock>
                    {
                        new Stock { Id = Guid.NewGuid(), BeerId = leffeBruneBeerId, WholesalerId = wholesalerId, Quantity = 20, Beer = leffeBruneBeer },
                        new Stock { Id = Guid.NewGuid(), BeerId = leffeTripleBeerId, WholesalerId = wholesalerId, Quantity = 10, Beer = leffeTripleBeer }
                    }
                }
            };

            var breweryContextMock = new Mock<BreweryContext>();
            breweryContextMock.Setup(x => x.Wholesalers).ReturnsDbSet(wholesalers);

            var wholesalerService = new WholesalerService(breweryContextMock.Object);

            // Act
            var result = await wholesalerService.CalculateEstimate(order);

            // Assert
            var expectedPriceWithoutVatAndDiscount = 3.5 * 4 + 5 * 3;

            result.IsSuccess.Should().BeTrue();
            result.Value.Discount.Should().Be(0);
            result.Value.TotalQuantity.Should().Be(7);
            result.Value.TotalPriceWithoutVatAndDiscount.Should().Be(expectedPriceWithoutVatAndDiscount);
            result.Value.TotalPriceWithVatAndDiscount.Should().Be(expectedPriceWithoutVatAndDiscount * 1.21);
            result.Value.Items.Should().HaveCount(2);

            var leffeBruneBeerEstimateItem = result.Value.Items.First(i => i.BeerId == leffeBruneBeerId);
            leffeBruneBeerEstimateItem.BeerId.Should().Be(leffeBruneBeerId);
            leffeBruneBeerEstimateItem.BeerName.Should().Be("Leffe Brune");
            leffeBruneBeerEstimateItem.Quantity.Should().Be(4);
            leffeBruneBeerEstimateItem.UnitPriceWithoutVat.Should().Be(3.5);
            leffeBruneBeerEstimateItem.TotalPriceWithoutVat.Should().Be(4 * 3.5);
        }

        [TestMethod]
        public async Task CalculateEstimate_Buy12Beers_ReturnsSuccessResultWith10PercentDiscount()
        {
            // Arrange
            var wholesalerId = new Guid("9D01DFC3-1227-427B-8E02-FCEC6522215B");
            var leffeBruneBeerId = new Guid("253F30AC-ACF7-4577-AD8F-F7252B0979C8");
            var leffeTripleBeerId = new Guid("F781DCCB-E4D9-4CD6-9684-0FDFB7E136CA");

            var order = new Order()
            {
                WholesalerId = wholesalerId,
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = leffeBruneBeerId, Quantity = 8 },
                    new OrderItem { BeerId = leffeTripleBeerId, Quantity = 4 }
                }
            };

            var leffeBruneBeer = new Beer()
            {
                Id = leffeBruneBeerId,
                Name = "Leffe Brune",
                PriceWithoutVat = 3.5
            };

            var leffeTripleBeer = new Beer()
            {
                Id = leffeBruneBeerId,
                Name = "Leffe Triple",
                PriceWithoutVat = 5
            };

            var wholesalers = new List<Wholesaler>
            {
                new Wholesaler
                {
                    Id = wholesalerId,
                    Name = "Test Wholesaler",
                    Stocks = new List<Stock>
                    {
                        new Stock { Id = Guid.NewGuid(), BeerId = leffeBruneBeerId, WholesalerId = wholesalerId, Quantity = 20, Beer = leffeBruneBeer },
                        new Stock { Id = Guid.NewGuid(), BeerId = leffeTripleBeerId, WholesalerId = wholesalerId, Quantity = 10, Beer = leffeTripleBeer }
                    }
                }
            };

            var breweryContextMock = new Mock<BreweryContext>();
            breweryContextMock.Setup(x => x.Wholesalers).ReturnsDbSet(wholesalers);

            var wholesalerService = new WholesalerService(breweryContextMock.Object);

            // Act
            var result = await wholesalerService.CalculateEstimate(order);

            // Assert
            const double expectedPriceWithoutVatAndDiscount = 3.5 * 8 + 5 * 4;

            result.IsSuccess.Should().BeTrue();
            result.Value.Discount.Should().Be(0.1);
            result.Value.TotalQuantity.Should().Be(12);
            result.Value.TotalPriceWithoutVatAndDiscount.Should().Be(expectedPriceWithoutVatAndDiscount);
            result.Value.TotalPriceWithVatAndDiscount.Should().Be(expectedPriceWithoutVatAndDiscount * 1.21 * 0.9);
        }

        [TestMethod]
        public async Task CalculateEstimate_Buy48Beers_ReturnsSuccessResultWith20PercentDiscount()
        {
            // Arrange
            var wholesalerId = new Guid("9D01DFC3-1227-427B-8E02-FCEC6522215B");
            var leffeBruneBeerId = new Guid("253F30AC-ACF7-4577-AD8F-F7252B0979C8");
            var leffeTripleBeerId = new Guid("F781DCCB-E4D9-4CD6-9684-0FDFB7E136CA");

            var order = new Order()
            {
                WholesalerId = wholesalerId,
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = leffeBruneBeerId, Quantity = 24 },
                    new OrderItem { BeerId = leffeTripleBeerId, Quantity = 24 }
                }
            };

            var leffeBruneBeer = new Beer()
            {
                Id = leffeBruneBeerId,
                Name = "Leffe Brune",
                PriceWithoutVat = 3.5
            };

            var leffeTripleBeer = new Beer()
            {
                Id = leffeBruneBeerId,
                Name = "Leffe Triple",
                PriceWithoutVat = 5
            };

            var wholesalers = new List<Wholesaler>
            {
                new Wholesaler
                {
                    Id = wholesalerId,
                    Name = "Test Wholesaler",
                    Stocks = new List<Stock>
                    {
                        new Stock { Id = Guid.NewGuid(), BeerId = leffeBruneBeerId, WholesalerId = wholesalerId, Quantity = 50, Beer = leffeBruneBeer },
                        new Stock { Id = Guid.NewGuid(), BeerId = leffeTripleBeerId, WholesalerId = wholesalerId, Quantity = 50, Beer = leffeTripleBeer }
                    }
                }
            };

            var breweryContextMock = new Mock<BreweryContext>();
            breweryContextMock.Setup(x => x.Wholesalers).ReturnsDbSet(wholesalers);

            var wholesalerService = new WholesalerService(breweryContextMock.Object);

            // Act
            var result = await wholesalerService.CalculateEstimate(order);

            // Assert
            const double expectedPriceWithoutVatAndDiscount = 3.5 * 24 + 5 * 24;

            result.IsSuccess.Should().BeTrue();
            result.Value.Discount.Should().Be(0.2);
            result.Value.TotalQuantity.Should().Be(48);
            result.Value.TotalPriceWithoutVatAndDiscount.Should().Be(expectedPriceWithoutVatAndDiscount);
            result.Value.TotalPriceWithVatAndDiscount.Should().Be(expectedPriceWithoutVatAndDiscount * 1.21 * 0.8);
        }

        #endregion
    }
}