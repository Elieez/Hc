using HealthCareABApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthCareABApi.Repositories.Implementations
{
	public class FeedbackRepository : IFeedbackRepository
    {
		private readonly AppDbContext _context;

		public FeedbackRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Feedback>> GetAllAsync()
        {
			return await _context.Feedbacks.ToListAsync();
		}

        public async Task<Feedback> GetByIdAsync(int id)
        {
			return await _context.Feedbacks.FindAsync(id);
		}

        public async Task CreateAsync(Feedback feedback)
        {
			await _context.Feedbacks.AddAsync(feedback);
		}

        public async Task UpdateAsync(int id, Feedback feedback)
        {
			_context.Feedbacks.Update(feedback);
			await _context.SaveChangesAsync();
		}

        public async Task DeleteAsync(int id)
        {
			var feedback = await _context.Feedbacks.FindAsync(id);
			if (feedback != null)
			{
				_context.Feedbacks.Remove(feedback);
				await _context.SaveChangesAsync();
			}
		}
    }
}

