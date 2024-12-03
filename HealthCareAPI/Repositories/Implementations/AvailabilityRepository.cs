using System.Collections.Generic;
using System.Linq;using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HealthCareABApi.Models;
using HealthCareABApi.Repositories.Implementations;
using System.Diagnostics.Eventing.Reader;

namespace HealthCareABApi.Repositories
{
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private readonly AppDbContext _context;

        public AvailabilityRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all availabilities
        public async Task<IEnumerable<Availability>> GetAllAsync()
        {
            return await _context.Availabilities.ToListAsync();
        }

        // Get availability by its Id
        public async Task<Availability> GetByIdAsync(int id)
        {
            return await _context.Availabilities
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        // Get all availabilities for a specific caregiver
        public async Task<IEnumerable<Availability>> GetAvailabilitiesAsync(int caregiverId)
        {
            return await _context.Availabilities
                .Where(a => a.CaregiverId == caregiverId)
                .ToListAsync();
        }

        // Add a new availability
        public async Task AddAvailabilityAsync(Availability availability)
        {
            await _context.Availabilities.AddAsync(availability);
            await _context.SaveChangesAsync();
        }

        // Delete an availability by its Id
        public async Task DeleteAvailabilityAsync(int id)
        {
            var availability = await _context.Availabilities.FindAsync(id);
            if (availability != null)
            {
                _context.Availabilities.Remove(availability);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(int id, Availability availability)
        {
            _context.Availabilities.Update(availability);
            await _context.SaveChangesAsync();
        }

        public async Task<Availability> GetByCaregiverIdAndDateAsync(int caregiverId, DateTime date)
        {
            return await _context.Availabilities
                .Where(a => a.CaregiverId == caregiverId && a.AvailableSlots.Any(slot => slot.Date == date))
                .FirstOrDefaultAsync();
        }
    }
}
