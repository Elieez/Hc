using HealthCareABApi.Models;
using HealthCareABApi.Repositories;

namespace HealthCareABApi.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentService(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Appointment> GetByIdAsync(int id)
        {
            var appointment = await _repository.GetByIdAsync(id);
            if (appointment == null)
            {
                throw new KeyNotFoundException($"Appointment with ID {id} not found.");
            }
            return appointment;
        }

        public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId)
        {
            var appointments = await _repository.GetByPatientIdAsync(patientId);
            if (appointments == null || !appointments.Any())
                throw new KeyNotFoundException($"No appointments found for patient with ID {patientId}.");

            return appointments;
        }

        public async Task<IEnumerable<Appointment>> GetByCaregiverIdAsync(int caregiverId)
        {
            var appointments = await _repository.GetByCaregiverIdAsync(caregiverId);
            if (appointments == null || !appointments.Any())
                throw new KeyNotFoundException($"No appointments found for caregiver with ID {caregiverId}.");

            return appointments;
        }

        public async Task CreateAsync(Appointment appointment)
        {
            if (appointment == null)
                throw new ArgumentNullException("Invalid appointment data.");

            await _repository.CreateAsync(appointment);
        }

        public async Task UpdateAsync(int id, Appointment appointment)
        {
            if (appointment == null || id != appointment.Id)
                throw new ArgumentException("Invalid appointment data or ID mismatch.");

            var existingAppointment = await _repository.GetByIdAsync(id);
            if (existingAppointment == null)
                throw new KeyNotFoundException($"Appointment with ID {id} not found.");

            await _repository.UpdateAsync(id, appointment);
        }

        public async Task DeleteAsync(int id)
        {
            var existingAppointment = await _repository.GetByIdAsync(id);
            if (existingAppointment == null)
                throw new KeyNotFoundException($"Appointment with ID {id} not found.");

            await _repository.DeleteAsync(id);
        }




    }
}
