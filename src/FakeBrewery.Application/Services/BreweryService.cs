using System;
using System.Threading.Tasks;
using FakeBrewery.Application.Interfaces;
using FakeBrewery.Domain.Models;
using FakeBrewery.Infra.Data;

namespace FakeBrewery.Application.Services
{
    public class BreweryService : IBreweryService
    {
        private readonly BreweryContext _context;

        public BreweryService(BreweryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>Add a new beer for a specific brewery.</summary>
        /// <param name="newBeer">The new beer to add.</param>
        /// <returns>
        ///     A success result with the new beer as value.<br />
        ///     A failure result with validation as error code if the beer has params validation errors.
        /// </returns>
        public async Task<Result<Beer>> AddNewBeer(Beer newBeer)
        {
            if (Validator.IsNullOrEmpty(newBeer.Name))
                return Result.Failure(newBeer, ResultErrorCode.Validation, "The name should not be empty.");
            if(Validator.IsGreaterOrEqualThanZero(newBeer.Strength))
                return Result.Failure(newBeer, ResultErrorCode.Validation, "The strength should be greater or equal than zero.");
            if(Validator.IsGreaterThanZero(newBeer.PriceWithoutVat))
                return Result.Failure(newBeer, ResultErrorCode.Validation, "The price should be greater than zero.");
            if(Validator.IsEmptyGuid(newBeer.BreweryId))
                return Result.Failure(newBeer, ResultErrorCode.Validation, "The brewery id should not be empty.");

            await _context.Beers.AddAsync(newBeer);
            await _context.SaveChangesAsync();

            return Result.Success(newBeer);
        }
    }
}