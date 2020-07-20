using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeBrewery.Domain.Models;

namespace FakeBrewery.Application.Interfaces
{
    public interface IBreweryService
    {
        /// <summary>Get all beers of a brewery with their wholesalers.</summary>
        /// <param name="breweryId">The brewery id.</param>
        /// <returns>
        ///     A success result with all beers as value.<br />
        ///     A failure result with Validation as error code if breweryId is an empty Guid.<br />
        ///     A failure result with NotFound as error code if the brewery does not exist.
        /// </returns>
        Task<Result<IEnumerable<Beer>>> GetBeersByBreweryAsync(Guid breweryId);

        /// <summary>Add a new beer for a specific brewery.</summary>
        /// <param name="newBeer">The new beer to add.</param>
        /// <returns>
        ///     A success result with the new beer as value.<br />
        ///     A failure result with Validation as error code if newBeer has params validation errors.<br />
        ///     A failure result with NotFound as error code if the brewery does not exist.
        /// </returns>
        Task<Result<Beer>> AddBeerAsync(Beer newBeer);

        /// <summary>Delete a specific beer</summary>
        /// <param name="beerId">The id of the beer to delete</param>
        /// <returns>
        ///     A success result with the beer as value.<br />
        ///     A failure result with Validation as error code if beerId is an empty Guid.<br />
        ///     A failure result with NotFound as error code if the beer was not found.
        /// </returns>
        Task<Result<Beer>> DeleteBeerAsync(Guid beerId);
    }
}