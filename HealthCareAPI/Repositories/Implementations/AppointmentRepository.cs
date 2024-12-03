using HealthCareABApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthCareABApi.Repositories.Implementations
{
	public class AppointmentRepository : IAppointmentRepository
	{
		private readonly AppDbContext _context;

		public AppointmentRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Appointment>> GetAllAsync()
		{
			return await _context.Appointments
			   .ToListAsync();
		}

		public async Task<Appointment> GetByIdAsync(int id)
		{
			return await _context.Appointments
				.FirstOrDefaultAsync(a => a.Id == id);
		}

        public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Appointments.Where(a => a.PatientId == patientId).ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByCaregiverIdAsync(int caregiverId)
        {
            return await _context.Appointments.Where(a => a.CaregiverId == caregiverId).ToListAsync();
        }


        public async Task CreateAsync(Appointment appointment)
		{
			await _context.Appointments.AddAsync(appointment);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(int id, Appointment appointment)
		{
			_context.Appointments.Update(appointment);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var appointment = await _context.Appointments.FindAsync(id);
			if (appointment != null)
			{
				_context.Appointments.Remove(appointment);
				await _context.SaveChangesAsync();
			}
		}

    }
}
