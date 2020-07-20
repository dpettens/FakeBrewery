using System.Threading.Tasks;
using FakeBrewery.Domain.Models;

namespace FakeBrewery.Application.Interfaces
{
    public interface IWholesalerService
    {
        /// <summary>Add a new stock of a beer for a specific wholesaler.</summary>
        /// <param name="newStock">The new stock to add.</param>
        /// <returns>
        ///     A success result with the new stock as value.<br />
        ///     A failure result with Validation as error code if newStock has params validation errors.<br />
        ///     A failure result with NotFound as error code if the wholesaler or the beer does not exist.
        /// </returns>
        Task<Result<Stock>> AddStockAsync(Stock newStock);
    }
}