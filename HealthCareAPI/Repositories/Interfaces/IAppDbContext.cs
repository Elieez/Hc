using HealthCareABApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthCareABApi.Repositories.Interfaces
{
	public interface IAppDbContext
	{
		DbSet<Appointment> Appointments { get; set; }
		DbSet<Availability> Availabilities { get; set; }
		DbSet<Feedback> Feedbacks { get; set; }
		DbSet<User> Users { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
