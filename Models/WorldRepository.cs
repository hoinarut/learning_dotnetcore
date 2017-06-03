using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            var trips = _context.Trips.ToList();
            _logger.LogInformation($"Found {trips.Count} trips");
            return trips;
        }
    }
}