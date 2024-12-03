using HealthCareABApi.DTO;
using HealthCareABApi.Models;
using HealthCareABApi.Repositories;
using HealthCareABApi.Repositories.Interfaces;
using HealthCareABApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCareABApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityRepository _availabilityRepository;

        public AvailabilityController(IAvailabilityRepository availabilityRepository)
        {
            _availabilityRepository = availabilityRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Availability>>> GetAllAvailabilities()
        {
            var availabilities = await _availabilityRepository.GetAllAsync();
            return Ok(availabilities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Availability>> GetAvailabilityById(int id)
        {
            var availability = await _availabilityRepository.GetByIdAsync(id);
            if (availability == null)
                return NotFound($"Availability with ID {id} not found.");

            return Ok(availability);
        }

        [HttpGet("caregiver/{caregiverId}")]
        public async Task<ActionResult<IEnumerable<Availability>>> GetAvailabilitiesByCaregiverId(int caregiverId)
        {
            var availabilities = await _availabilityRepository.GetAvailabilitiesAsync(caregiverId);
            return Ok(availabilities);
        }

        [HttpPost]
        public async Task<IActionResult> AddAvailability([FromBody] Availability availability)
        {
            if (availability == null)
                return BadRequest("Invalid availability data.");

            foreach (var slot in availability.AvailableSlots)
            {
                slot.Date = DateTime.SpecifyKind(slot.Date, DateTimeKind.Utc);
            }

            await _availabilityRepository.AddAvailabilityAsync(availability);
            return CreatedAtAction(nameof(GetAvailabilityById), new { id = availability.Id }, availability);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvailability(int id)
        {
            var availability = await _availabilityRepository.GetByIdAsync(id);
            if (availability == null)
                return NotFound($"Availability with ID {id} not found.");

            await _availabilityRepository.DeleteAvailabilityAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAvailability(int id, [FromBody] Availability availability)
        {
            if (availability == null || id != availability.Id)
                return BadRequest("Invalid availability data or mismatched ID.");

            var existingAvailability = await _availabilityRepository.GetByIdAsync(id);
            if (existingAvailability == null)
                return NotFound($"Availability with ID {id} not found.");

            await _availabilityRepository.UpdateAsync(id, availability);
            return NoContent();
        }

        [HttpPut("{id}/update-slot")]
        public async Task<IActionResult> UpdateAvailabilitySlot(int id, [FromBody] DateTime slotDate)
        {
            var availability = await _availabilityRepository.GetByIdAsync(id);
            if (availability == null)
            {
                return NotFound($"Availability with ID {id} not found.");
            }

            var originalCount = availability.AvailableSlots.Count;
            availability.AvailableSlots.RemoveAll(slot => slot.Date == slotDate);

            if (availability.AvailableSlots.Count == 0)
            {
                await _availabilityRepository.DeleteAvailabilityAsync(id);
            }
            else
            {
                await _availabilityRepository.UpdateAsync(id, availability);
            }

            return NoContent();
        }




    }
}
