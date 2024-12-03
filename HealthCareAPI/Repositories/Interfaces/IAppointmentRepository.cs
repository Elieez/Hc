using System;
using HealthCareABApi.Models;

namespace HealthCareABApi.Repositories
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment> GetByIdAsync(int id);
        Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId);
        Task<IEnumerable<Appointment>> GetByCaregiverIdAsync(int caregiverId);
        Task CreateAsync(Appointment appointment);
        Task UpdateAsync(int id, Appointment appointment);
        Task DeleteAsync(int id);
    }
}

