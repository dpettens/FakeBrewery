using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeBrewery.Application.Interfaces;
using FakeBrewery.Domain.Models;
using FakeBrewery.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace FakeBrewery.Application.Services
{
    public class BreweryService : IBreweryService
    {
        private readonly BreweryContext _context;

        public BreweryService(BreweryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>Get all beers of a brewery with their wholesalers.</summary>
        /// <param name="breweryId">The brewery id.</param>
        /// <returns>
        ///     A success result with all beers as value.<br />
        ///     A failure result with Validation as error code if breweryId is an empty Guid.<br />
        ///     A failure result with NotFound as error code if the brewery does not exist.
        /// </returns>
        public async Task<Result<IEnumerable<Beer>>> GetBeersByBreweryAsync(Guid breweryId)
        {
            if (Validator.IsEmptyGuid(breweryId))
                return Result.Failure<IEnumerable<Beer>>(null, ResultErrorCode.Validation, "The brewery id should not be empty.");

            var brewery = await _context.Breweries.FindAsync(breweryId);
            if (brewery == null)
                return Result.Failure<IEnumerable<Beer>>(null, ResultErrorCode.NotFound, $"The brewery with {breweryId} was not found.");

            var beers = _context.Beers
                .Where(b => b.BreweryId == breweryId)
                .Include(b => b.Brewery)
                .Include(b => b.Stocks)
                .ThenInclude(s => s.Wholesaler)
                .AsEnumerable();

            return Result.Success(beers);
        }

        /// <summary>Add a new beer for a specific brewery.</summary>
        /// <param name="newBeer">The new beer to add.</param>
        /// <returns>
        ///     A success result with the new beer as value.<br />
        ///     A failure result with Validation as error code if newBeer has params validation errors.<br />
        ///     A failure result with NotFound as error code if the brewery does not exist.
        /// </returns>
        public async Task<Result<Beer>> AddBeerAsync(Beer newBeer)
        {
            if (Validator.IsNullOrEmpty(newBeer.Name))
                return Result.Failure(newBeer, ResultErrorCode.Validation, "The name should not be empty.");
            if(!Validator.IsGreaterOrEqualThanZero(newBeer.Strength))
                return Result.Failure(newBeer, ResultErrorCode.Validation, "The strength should be greater or equal than zero.");
            if(!Validator.IsGreaterThanZero(newBeer.PriceWithoutVat))
                return Result.Failure(newBeer, ResultErrorCode.Validation, "The price should be greater than zero.");
            if(Validator.IsEmptyGuid(newBeer.BreweryId))
                return Result.Failure(newBeer, ResultErrorCode.Validation, "The brewery id should not be empty.");

            var brewery = await _context.Breweries.FindAsync(newBeer.BreweryId);
            if (brewery == null)
                return Result.Failure(newBeer, ResultErrorCode.NotFound, $"The brewery with {newBeer.BreweryId} was not found.");

            await _context.Beers.AddAsync(newBeer);
            await _context.SaveChangesAsync();

            return Result.Success(newBeer);
        }

        /// <summary>Delete a specific beer</summary>
        /// <param name="beerId">The id of the beer to delete</param>
        /// <returns>
        ///     A success result with the beer as value.<br />
        ///     A failure result with Validation as error code if beerId is an empty Guid.<br />
        ///     A failure result with NotFound as error code if the beer was not found.
        /// </returns>
        public async Task<Result<Beer>> DeleteBeerAsync(Guid beerId)
        {
            if (Validator.IsEmptyGuid(beerId))
                return Result.Failure<Beer>(null, ResultErrorCode.Validation, "The beer id should not be empty.");

            var beer = await _context.Beers.FindAsync(beerId);
            if (beer == null)
                return Result.Failure<Beer>(null, ResultErrorCode.NotFound, $"The beer with {beerId} id was not found.");

            _context.Beers.Remove(beer);
            await _context.SaveChangesAsync();

            return Result.Success(beer);
        }
    }
}