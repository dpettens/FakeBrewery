using System;
using System.Threading.Tasks;
using FakeBrewery.Domain.Models;

namespace FakeBrewery.Application.Interfaces
{
    public interface IBreweryService
    {
        /// <summary>Add a new beer for a specific brewery.</summary>
        /// <param name="newBeer">The new beer to add.</param>
        /// <returns>
        ///     A success result with the new beer as value.<br />
        ///     A failure result with Validation as error code if newBeer has params validation errors.
        /// </returns>
        Task<Result<Beer>> AddNewBeer(Beer newBeer);

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