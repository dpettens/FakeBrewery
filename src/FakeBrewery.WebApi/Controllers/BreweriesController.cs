using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeBrewery.Application;
using FakeBrewery.Application.Interfaces;
using FakeBrewery.Domain.Models;
using FakeBrewery.WebApi.ApiModels;
using Microsoft.AspNetCore.Mvc;

namespace FakeBrewery.WebApi.Controllers
{
    [Route("[controller]")]
    public class BreweriesController : ControllerBase
    {
        private readonly IBreweryService _breweryService;
        private readonly IMapper _mapper;

        public BreweriesController(IBreweryService breweryService, IMapper mapper)
        {
            _breweryService = breweryService ?? throw new ArgumentNullException(nameof(breweryService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{breweryId}/beers")]
        public async Task<ActionResult<BeerResponse>> GetBeersByBrewery(Guid breweryId)
        {
           var result = await _breweryService.GetBeersByBreweryAsync(breweryId);

           if (result.IsFailure && result.ErrorCode == ResultErrorCode.Validation)
               return UnprocessableEntity(result.ErrorMessage);

           if (result.IsFailure && result.ErrorCode == ResultErrorCode.NotFound)
               return NotFound(result.ErrorMessage);

           var response = _mapper.Map<IEnumerable<BeerResponse>>(result.Value);
           return Ok(response);
        }

        [HttpPost("{breweryId}/beers")]
        public async Task<ActionResult<Beer>> AddBeer(Guid breweryId, [FromBody] AddBeerRequest createBeerRequest)
        {
            createBeerRequest.BreweryId = breweryId;
            var result = await _breweryService.AddBeerAsync(_mapper.Map<Beer>(createBeerRequest));

            if (result.IsFailure && result.ErrorCode == ResultErrorCode.Validation)
                return UnprocessableEntity(result.ErrorMessage);

            if (result.IsFailure && result.ErrorCode == ResultErrorCode.NotFound)
                return NotFound(result.ErrorMessage);

            return Ok();
        }

        [HttpDelete("{breweryId}/beers/{beerId}")]
        public async Task<ActionResult> DeleteBeer(Guid beerId)
        {
            var result = await _breweryService.DeleteBeerAsync(beerId);

            if (result.IsFailure && result.ErrorCode == ResultErrorCode.Validation)
                return UnprocessableEntity(result.ErrorMessage);

            if (result.IsFailure && result.ErrorCode == ResultErrorCode.NotFound)
                return NotFound(result.ErrorMessage);

            return NoContent();
        }
    }
}
