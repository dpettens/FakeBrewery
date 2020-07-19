using System;
using System.Threading.Tasks;
using AutoMapper;
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
            var result = await _breweryService.AddNewBeer(_mapper.Map<Beer>(createBeerRequest));
            
            if (result.IsFailure)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }
    }
}
