using System.Threading.Tasks;
using FakeBrewery.Domain.Models;

namespace FakeBrewery.Application.Interfaces
{
    public interface IBreweryService
    {
        /// <summary>Add a new beer for a specific brewery.</summary>
        /// <param name="newBeer">The new beer to add.</param>
        /// <returns>
        ///     A success result with the new beer as value.<br/>
        ///     A failure result with validation as error code if the beer has params validation errors.
        /// </returns>
        Task<Result<Beer>> AddNewBeer(Beer newBeer);
    }
}