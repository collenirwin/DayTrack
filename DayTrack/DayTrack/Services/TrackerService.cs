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

        public async Task<Tracker> AddTrackerAsync(string name)
        {
            name = name ?? throw new ArgumentNullException(nameof(name));

            var tracker = new Tracker { Name = name };
            _context.Trackers.Add(tracker);
            await _context.SaveChangesAsync();
            return tracker;
        }

        public async Task<Tracker> TryAddTrackerAsync(string name) =>
            await Try.RunAsync(async () => await AddTrackerAsync(name),
                ex => _logger.Error(ex, $"Failed to add tracker with name {name}."));

        public async Task<IEnumerable<Tracker>> GetAllTrackersAsync() =>
            await _context.Trackers
                .OrderBy(tracker => tracker.Name)
                .ToListAsync();

        public async Task<IEnumerable<Tracker>> TryGetAllTrackersAsync() =>
            await Try.RunAsync(GetAllTrackersAsync,
                ex => _logger.Error(ex, "Failed to get all trackers."));

        public async Task DeleteTrackerAsync(int id)
        {
            var tracker = await _context.Trackers.FirstAsync(t => t.Id == id);
            _context.Trackers.Remove(tracker);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> TryDeleteTrackerAsync(int id) =>
            await Try.RunAsync(async () => await DeleteTrackerAsync(id),
                ex => _logger.Error(ex, $"Failed to delete tracker with id {id}."));

        public async Task<Tracker> UpdateTrackerNameAsync(int id, string name)
        {
            var tracker = await _context.Trackers.FirstAsync(t => t.Id == id);
            tracker.Name = name;
            await _context.SaveChangesAsync();
            return tracker;
        }

        public async Task<Tracker> TryUpdateTrackerNameAsync(int id, string name) =>
            await Try.RunAsync(async () => await UpdateTrackerNameAsync(id, name),
                ex => _logger.Error(ex, $"Failed to update tracker with id {id}'s name to {name}."));
    }
}
