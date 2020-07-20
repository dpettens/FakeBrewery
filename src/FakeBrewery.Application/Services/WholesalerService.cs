using System;
using System.Threading.Tasks;
using FakeBrewery.Application.Interfaces;
using FakeBrewery.Domain.Models;
using FakeBrewery.Infra.Data;

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
            if (!Validator.IsGreaterOrEqualThanZero(newStock.Quantity))
                return Result.Failure(newStock, ResultErrorCode.Validation, "The quantity should be greater or equal than zero.");
            if (Validator.IsEmptyGuid(newStock.WholesalerId))
                return Result.Failure(newStock, ResultErrorCode.Validation, "The wholesaler id should not be empty.");
            if (Validator.IsEmptyGuid(newStock.BeerId))
                return Result.Failure(newStock, ResultErrorCode.Validation, "The beer id should not be empty.");

            var wholesaler = await _context.Wholesalers.FindAsync(newStock.WholesalerId);
            if (wholesaler == null)
                return Result.Failure(newStock, ResultErrorCode.NotFound, $"The wholesaler with {newStock.WholesalerId} was not found.");

            var beer = await _context.Beers.FindAsync(newStock.BeerId);
            if (beer == null)
                return Result.Failure(newStock, ResultErrorCode.NotFound, $"The beer with {newStock.BeerId} was not found.");

            await _context.Stocks.AddAsync(newStock);
            await _context.SaveChangesAsync();

            return Result.Success(newStock);
        }
    }
}