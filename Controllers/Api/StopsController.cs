using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("/api/trips/{tripName}/stops")]
    [Authorize]
    public class StopsController : Controller
    {
        private IWorldRepository _repository;
        private ILogger<StopsController> _logger;
        private GeoCoordService _geoService;

        public StopsController(IWorldRepository repository
        , ILogger<StopsController> logger
        , GeoCoordService geoService)
        {
            _repository = repository;
            _logger = logger;
            _geoService = geoService;
        }

        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _repository.GetUserTripByName(tripName, this.User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order)));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get stops: {ex}");
            }
            return BadRequest($"Could not retrieve stops for trip {tripName}");
        }

        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName, [FromBody]StopViewModel stop)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(stop);

                    var result = await _geoService.GetCoordsAsync(newStop.Name);
                    if (!result.Success)
                    {
                        _logger.LogError(result.Message);
                    }
                    else
                    {
                        newStop.Latitude = result.Latitude;
                        newStop.Longitude = result.Longitude;
                        _repository.AddStop(tripName, newStop, User.Identity.Name);
                        if (await _repository.SaveChangesAsync())
                        {
                            return Created($"/api/trips/{tripName}/stops/{newStop.Name}",
                            Mapper.Map<StopViewModel>(newStop));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save new Stop: {ex}");
            }
            return BadRequest("Failed to save new stop");
        }
    }
}