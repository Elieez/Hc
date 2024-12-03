using System;
using HealthCareABApi.Models;

namespace HealthCareABApi.Repositories
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<Feedback>> GetAllAsync();
        Task<Feedback> GetByIdAsync(int id);
        Task CreateAsync(Feedback feedback);
        Task UpdateAsync(int id, Feedback feedback);
        Task DeleteAsync(int id);
    }
}

