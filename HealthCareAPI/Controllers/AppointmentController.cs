using HealthCareABApi.Models;
using HealthCareABApi.Repositories;
using HealthCareABApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthCareABApi.Controllers
{
    // Route definition for the API controller
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        // Dependency injection of the AppointmentService into the controller
        private readonly AppointmentService _service;
        public AppointmentsController(AppointmentService service)
        {
            _service = service;  // Assigning the injected service to the controller's private field
        }

        // Endpoint to create a new appointment
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] Appointment appointment)
        {
            if (appointment == null)  // Check if the appointment data is null
                return BadRequest("Invalid appointment data.");  // Return a BadRequest if data is invalid

            try
            {
                // Calling the service layer to create the appointment
                await _service.CreateAsync(appointment);
                return Ok("Appointment created successfully.");  // Return a success response
            }
            catch (Exception ex)  // Catch any exceptions that might occur during appointment creation
            {
                return BadRequest(new { message = ex.Message });  // Return a BadRequest with the exception message
            }
        }

        // Endpoint to retrieve an appointment by its ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(int id)
        {
            try
            {
                // Calling the service layer to fetch the appointment by its ID
                var appointment = await _service.GetByIdAsync(id);
                return Ok(appointment);  // Return the appointment if found
            }
            catch (KeyNotFoundException ex)  // Catch the exception if the appointment is not found
            {
                return NotFound(new { message = ex.Message });  // Return a NotFound response with the exception message
            }
        }

        // Endpoint to retrieve all appointments
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAppointments()
        {
            // Calling the service layer to fetch all appointments
            var appointments = await _service.GetAllAsync();
            return Ok(appointments);  // Return the list of all appointments
        }

        // Endpoint to retrieve appointments by a specific patient ID
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetAppointmentsByPatientId(int patientId)
        {
            // Calling the service layer to fetch appointments by patient ID
            var appointments = await _service.GetByPatientIdAsync(patientId);
            if (appointments == null || !appointments.Any())  // If no appointments found, return NotFound
                return NotFound($"No appointments found for patient with ID {patientId}.");
            return Ok(appointments);  // Return the list of appointments for the patient
        }

        // Endpoint to retrieve appointments by a specific caregiver ID
        [HttpGet("caregiver/{caregiverId}")]
        public async Task<IActionResult> GetAppointmentsByCaregiverId(int caregiverId)
        {
            // Calling the service layer to fetch appointments by caregiver ID
            var appointments = await _service.GetByCaregiverIdAsync(caregiverId);
            if (appointments == null || !appointments.Any())  // If no appointments found, return NotFound
                return NotFound($"No appointments found for caregiver with ID {caregiverId}.");
            return Ok(appointments);  // Return the list of appointments for the caregiver
        }

        // Endpoint to update an existing appointment
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] Appointment appointment)
        {
            if (appointment == null || id != appointment.Id)  // Check if the data is invalid or IDs mismatch
                return BadRequest("Invalid appointment data or ID mismatch.");  // Return a BadRequest for invalid data

            try
            {
                // Calling the service layer to update the appointment
                await _service.UpdateAsync(id, appointment);
                return Ok("Appointment updated successfully.");  // Return success response after update
            }
            catch (KeyNotFoundException ex)  // Catch the exception if the appointment is not found
            {
                return NotFound(new { message = ex.Message });  // Return NotFound with exception message
            }
            catch (ArgumentException ex)  // Catch argument mismatch exception
            {
                return BadRequest(new { message = ex.Message });  // Return BadRequest with exception message
            }
        }

        // Endpoint to delete an appointment by its ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                // Calling the service layer to delete the appointment
                await _service.DeleteAsync(id);
                return Ok("Appointment deleted successfully.");  // Return success response after deletion
            }
            catch (KeyNotFoundException ex)  // Catch exception if the appointment is not found
            {
                return NotFound(new { message = ex.Message });  // Return NotFound with exception message
            }
        }
    }
}
