using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public void AddStop(string tripName, Stop stop, string userName)
        {
            var trip = GetUserTripByName(tripName, userName);
            if (trip != null)
            {
                trip.Stops.Add(stop);
                _context.Stops.Add(stop);
            }
        }

        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            var trips = _context.Trips.ToList();
            _logger.LogInformation($"Found {trips.Count} trips");
            return trips;
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .FirstOrDefault(t => t.Name == tripName);
        }

        public IEnumerable<Trip> GetTripsByUserName(string userName)
        {
            var trips = _context.Trips.Where(t => t.UserName == userName).ToList();
            return trips;
        }

        public Trip GetUserTripByName(string tripName, string userName)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .FirstOrDefault(t => t.Name == tripName && t.UserName == userName);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}