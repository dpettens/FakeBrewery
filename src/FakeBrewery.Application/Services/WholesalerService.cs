using System;
using System.Linq;
using System.Threading.Tasks;
using FakeBrewery.Application.Dtos;
using FakeBrewery.Application.Interfaces;
using FakeBrewery.Application.Specifications;
using FakeBrewery.Domain.Models;
using FakeBrewery.Domain.Specifications;
using FakeBrewery.Infra.Data;
using Microsoft.EntityFrameworkCore;
using NSpecifications;

namespace FakeBrewery.Application.Services
{
    public class WholesalerService : IWholesalerService
    {
        private readonly BreweryContext _context;

        public WholesalerService(BreweryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>Add a new stock of a beer for a specific wholesaler.</summary>
        /// <param name="newStock">The new stock to add.</param>
        /// <returns>
        ///     A success result with the new stock as value.<br />
        ///     A failure result with Validation as error code if newStock has params validation errors.<br />
        ///     A failure result with NotFound as error code if the wholesaler or the beer does not exist.
        /// </returns>
        public async Task<Result<Stock>> AddStockAsync(Stock newStock)
        {
            if (!newStock.Is(StockSpecifications.HasValidQuantity))
                return Result.Failure(newStock, ResultErrorCode.Validation, "The quantity should be greater or equal than zero.");
            if (!newStock.Is(StockSpecifications.HasValidWholesalerId))
                return Result.Failure(newStock, ResultErrorCode.Validation, "The wholesaler id should not be empty.");
            if (!newStock.Is(StockSpecifications.HasValidBeerId))
                return Result.Failure(newStock, ResultErrorCode.Validation, "The beer id should not be empty.");

            var wholesaler = await _context.Wholesalers.FindAsync(newStock.WholesalerId);
            if (wholesaler == null)
                return Result.Failure(newStock, ResultErrorCode.NotFound, $"The wholesaler with {newStock.WholesalerId} id was not found.");

            var beer = await _context.Beers.FindAsync(newStock.BeerId);
            if (beer == null)
                return Result.Failure(newStock, ResultErrorCode.NotFound, $"The beer with {newStock.BeerId}id  was not found.");

            await _context.Stocks.AddAsync(newStock);
            await _context.SaveChangesAsync();

            return Result.Success(newStock);
        }

        /// <summary>Update the stock of a beer for a specific wholesaler.</summary>
        /// <param name="stockToUpdate">The stock to update.</param>
        /// <returns>
        ///     A success result with the new stock as value.<br />
        ///     A failure result with Validation as error code if stock has params validation errors.<br />
        ///     A failure result with NotFound as error code if the stock does not exist.
        /// </returns>
        public async Task<Result<Stock>> UpdateStockAsync(Stock stockToUpdate)
        {
            if (!stockToUpdate.Is(StockSpecifications.HasValidId))
                return Result.Failure(stockToUpdate, ResultErrorCode.Validation, "The id should not be empty.");
            if (!stockToUpdate.Is(StockSpecifications.HasValidQuantity))
                return Result.Failure(stockToUpdate, ResultErrorCode.Validation, "The quantity should be greater or equal than zero.");

            var stock = await _context.Stocks.FindAsync(stockToUpdate.Id);
            if (stock == null)
                return Result.Failure(stockToUpdate, ResultErrorCode.NotFound, $"The stock with {stockToUpdate.Id} id was not found.");

            // Only the quantity can be updated
            stock.Quantity = stockToUpdate.Quantity;

            await _context.SaveChangesAsync();

            return Result.Success(stock);
        }

        /// <summary>Calculate an estimate from an order</summary>
        /// <param name="order">The order to calculate.</param>
        /// <returns>
        ///     A success result with the estimate as value.<br />
        ///     A failure result with Validation as error code if order has validation errors.<br />
        ///     A failure result with NotFound as error code if the wholesaler or any beers do not exist.<br />
        ///     A failure result with Business as error code if order asks for beers not sold by the wholesaler.
        /// </returns>
        public async Task<Result<Estimate>> CalculateEstimate(Order order)
        {
            if (order.Is(OrderSpecifications.IsEmpty))
                return Result.Failure<Estimate>(null, ResultErrorCode.Validation, "The wholesalerId or items should not be empty.");
            if (!order.Is(OrderSpecifications.IsAllItemsValid))
                return Result.Failure<Estimate>(null, ResultErrorCode.Validation, "At least one order item is invalid.");
            if (order.Is(OrderSpecifications.HasDuplicateItems))
                return Result.Failure<Estimate>(null, ResultErrorCode.Validation, "There is multiple duplicated items.");

            var wholesaler = await _context.Wholesalers
                .Include(w => w.Stocks)
                .ThenInclude(s => s.Beer)
                .FirstOrDefaultAsync(o => o.Id == order.WholesalerId);
            
            if (wholesaler == null)
                return Result.Failure<Estimate>(null, ResultErrorCode.NotFound, $"The wholesaler with {order.WholesalerId} id was not found.");

            var estimate = new Estimate();

            // Loop on all requested beers, check if the wholesaler sells them and has enough in stock
            // Add a detailed account for each beer with its unit price
            foreach (var item in order.Items)
            {
                var stock = wholesaler.Stocks.FirstOrDefault(s => s.BeerId == item.BeerId);

                if (stock == null)
                    return Result.Failure<Estimate>(null, ResultErrorCode.NotFound, $"The beer with {item.BeerId} id was not found in the wholesaler's stock.");

                if(item.Quantity > stock.Quantity)
                    return Result.Failure<Estimate>(null, ResultErrorCode.Business, $"The wholesaler does not have the quantity for beer with {item.BeerId} id.");

                estimate.Items.Add(new EstimateItem
                {
                    BeerId = item.BeerId,
                    Quantity = item.Quantity,
                    BeerName = stock.Beer.Name,
                    UnitPriceWithoutVat = stock.Beer.PriceWithoutVat
                });
            }

            // Apply a discount of 10% above 10 beers and 20% above 20 beers
            estimate.Discount = estimate.TotalQuantity < 10 ? 0 : estimate.TotalQuantity > 20 ? 0.2 : 0.1;

            return Result.Success(estimate);
        }
    }
}