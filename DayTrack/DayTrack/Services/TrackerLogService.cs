﻿using DayTrack.Data;
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
    public class TrackerLogService
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public TrackerLogService(AppDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task LogDayAsync(int trackerId, DateTime day)
        {
            _context.LoggedDays.Add(new LoggedDay
            {
                TrackerId = trackerId,
                Date = day
            });

            await _context.SaveChangesAsync();
        }

        public async Task<bool> TryLogDayAsync(int trackerId, DateTime day) =>
            await Try.RunAsync(async () => await LogDayAsync(trackerId, day),
                ex => _logger.Error(ex, $"Failed to log day (trackerId: {trackerId}, day: {day})."));

        public async Task<IEnumerable<LoggedDay>> GetAllLoggedDaysAsync(int trackerId) =>
            await _context.LoggedDays
                .Where(day => day.TrackerId == trackerId)
                .OrderByDescending(day => day.Date)
                .ToListAsync();

        public async Task<IEnumerable<LoggedDay>> TryGetAllLoggedDaysAsync(int trackerId) =>
            await Try.RunAsync(async () => await GetAllLoggedDaysAsync(trackerId),
                ex => _logger.Error(ex, $"Failed to get all logged days for tracker id {trackerId}."));
    }
}
