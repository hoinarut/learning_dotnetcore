using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWorld.Helpers;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    public class TripsController : Controller
    {
        private IWorldRepository _repository;
        private ILogger<TripsController> _logger;

        public TripsController(IWorldRepository repository, ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var results = _repository.GetAllTrips();
                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(results));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all trips: {ex}");
                return BadRequest("Error occurred");
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TripViewModel trip)
        {
            if (ModelState.IsValid)
            {
                var newTrip = Mapper.Map<Trip>(trip);
                _repository.AddTrip(newTrip);
                if (await _repository.SaveChangesAsync())
                {
                    return Created($"api/trips/{trip.Name}", Mapper.Map<TripViewModel>(newTrip));
                }
            }
            // return BadRequest($"Bad data: {ModelState.Errors()}");
            // return BadRequest(ModelState);
            return BadRequest("Failed to save the trip");
        }
    }
}