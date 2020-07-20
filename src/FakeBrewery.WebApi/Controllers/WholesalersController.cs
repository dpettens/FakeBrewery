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
    public class WholesalersController : ControllerBase
    {
        private readonly IWholesalerService _wholesalerService;
        private readonly IMapper _mapper;

        public WholesalersController(IWholesalerService wholesalerService, IMapper mapper)
        {
            _wholesalerService = wholesalerService ?? throw new ArgumentNullException(nameof(wholesalerService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("{wholesalerId}/stocks")]
        public async Task<ActionResult<Stock>> AddStock(Guid wholesalerId, [FromBody] AddStockRequest addStockRequest)
        {
            addStockRequest.WholesalerId = wholesalerId;
            var result = await _wholesalerService.AddStockAsync(_mapper.Map<Stock>(addStockRequest));

            if (result.IsFailure && result.ErrorCode == ResultErrorCode.Validation)
                return UnprocessableEntity(result.ErrorMessage);

            if (result.IsFailure && result.ErrorCode == ResultErrorCode.NotFound)
                return NotFound(result.ErrorMessage);

            return Ok();
        }
    }
}
