using DayTrack.Data;
using DayTrack.Models;
using DayTrack.Utils;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DayTrack.Services
{
    public class TrackerService
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public TrackerService(AppDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddTrackerAsync(string name)
        {
            name = name ?? throw new ArgumentNullException(nameof(name));

            _context.Trackers.Add(new Tracker { Name = name });
            await _context.SaveChangesAsync();
        }

        public async Task<bool> TryAddTrackerAsync(string name) =>
            await Try.RunAsync(async () => await AddTrackerAsync(name),
                ex => _logger.Error(ex, $"Failed to add tracker with name {name}."));

        public async Task<IEnumerable<Tracker>> GetAllTrackersAsync() =>
            await _context.Trackers
                .OrderBy(tracker => tracker.Name)
                .ToListAsync();

        public async Task<IEnumerable<Tracker>> TryGetAllTrackersAsync() =>
            await Try.RunAsync(GetAllTrackersAsync,
                ex => _logger.Error(ex, "Failed to get all trackers."));
    }
}
