using System.Collections.Generic;
using System.Threading.Tasks;
using HealthCareABApi.Models;

namespace HealthCareABApi.Repositories
{
    public interface IAvailabilityRepository
    {
        Task<IEnumerable<Availability>> GetAllAsync();                   
        Task<Availability> GetByIdAsync(int id);                         
        Task<IEnumerable<Availability>> GetAvailabilitiesAsync(int caregiverId); 
        Task AddAvailabilityAsync(Availability availability);            
        Task DeleteAvailabilityAsync(int id);                            
        Task UpdateAsync(int id, Availability availability);
        Task<Availability> GetByCaregiverIdAndDateAsync(int caregiverId, DateTime date);
    }
}
