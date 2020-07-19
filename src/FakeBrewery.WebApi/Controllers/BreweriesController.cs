using System;
using System.Threading.Tasks;
using AutoMapper;
using FakeBrewery.Application;
using FakeBrewery.Application.Interfaces;
using FakeBrewery.Domain.Models;
using FakeBrewery.WebApi.Dtos;
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

        [HttpPost("{breweryId}/beers")]
        public async Task<ActionResult<Beer>> AddBeer(Guid breweryId, [FromBody] AddBeerRequest createBeerRequest)
        {
            createBeerRequest.BreweryId = breweryId;
            var result = await _breweryService.AddBeerAsync(_mapper.Map<Beer>(createBeerRequest));

            if (result.IsFailure && result.ErrorCode == ResultErrorCode.Validation)
                return BadRequest(result.ErrorMessage);

            if (result.IsFailure && result.ErrorCode == ResultErrorCode.NotFound)
                return NotFound(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpDelete("{breweryId}/beers/{beerId}")]
        public async Task<ActionResult> DeleteBeer(Guid beerId)
        {
            var result = await _breweryService.DeleteBeerAsync(beerId);

            if (result.IsFailure && result.ErrorCode == ResultErrorCode.Validation)
                return BadRequest(result.ErrorMessage);

            if (result.IsFailure && result.ErrorCode == ResultErrorCode.NotFound)
                return NotFound(result.ErrorMessage);

            return NoContent();
        }
    }
}
