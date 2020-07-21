using System;
using System.Threading.Tasks;
using AutoMapper;
using FakeBrewery.Application;
using FakeBrewery.Application.Dtos;
using FakeBrewery.Application.Interfaces;
using FakeBrewery.Domain.Models;
using FakeBrewery.WebApi.ApiModels;
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

        [HttpPatch("{wholesalerId}/stocks/{stockId}")]
        public async Task<ActionResult<Stock>> UpdateStock(Guid stockId, [FromBody] UpdateStockRequest updateStockRequest)
        {
            updateStockRequest.Id = stockId;
            var result = await _wholesalerService.UpdateStockAsync(_mapper.Map<Stock>(updateStockRequest));

            if (result.IsFailure && result.ErrorCode == ResultErrorCode.Validation)
                return UnprocessableEntity(result.ErrorMessage);

            if (result.IsFailure && result.ErrorCode == ResultErrorCode.NotFound)
                return NotFound(result.ErrorMessage);

            return Ok();
        }

        [HttpPost("{wholesalerId}/estimates")]
        public async Task<ActionResult<Stock>> CalculateEstimate(Guid wholesalerId, [FromBody] Order order)
        {
            order.WholesalerId = wholesalerId;
            var result = await _wholesalerService.CalculateEstimate(order);

            if (result.IsFailure && (result.ErrorCode == ResultErrorCode.Validation || result.ErrorCode == ResultErrorCode.Business))
                return UnprocessableEntity(result.ErrorMessage);

            if (result.IsFailure && result.ErrorCode == ResultErrorCode.NotFound)
                return NotFound(result.ErrorMessage);

            return Ok(result.Value);
        }
    }
}
