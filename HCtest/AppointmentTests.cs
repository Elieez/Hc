using HealthCareABApi.Controllers;
using HealthCareABApi.Models;
using HealthCareABApi.Repositories;
using HealthCareABApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HCtest;

public class AppointmentTests
{
    private readonly Mock<IAppointmentRepository> _mockRepository;
    private readonly Mock<AppointmentService> _mockService;
    private readonly AppointmentsController _controller;

    public AppointmentTests()
    {
        // Mock the appointment repository
        _mockRepository = new Mock<IAppointmentRepository>();

        // Mock the appointment service using the mocked repository
        _mockService = new Mock<AppointmentService>(_mockRepository.Object);

        // Initialize the controller with the mocked service
        _controller = new AppointmentsController(_mockService.Object);
    }

    [Fact]
    public async Task AddAppointment_ShouldReturnBadRequest_WhenInvalidData()
    {
        // Arrange: Set up invalid data (null appointment)
        Appointment invalidAppointment = null;

        // Act: Call the CreateAppointment method with the invalid data
        var result = await _controller.CreateAppointment(invalidAppointment);

        // Assert: Verify that a BadRequestObjectResult is returned with the correct error message
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid appointment data.", badRequestResult.Value);

        // Ensure the repository's CreateAsync method is never called
        _mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<Appointment>()), Times.Never);
    }

    [Fact]
    public async Task AddAppointment_ShouldReturnBadRequest_WhenMissingCaregiverId()
    {
        // Arrange: Create an appointment with an invalid CaregiverId (0)
        var invalidAppointment = new Appointment
        {
            PatientId = 1,
            CaregiverId = 0, // Invalid value (CaregiverId is required)
            DateTime = DateTime.UtcNow.AddDays(1),
            Status = AppointmentStatus.Scheduled
        };

        // Act: Call the CreateAppointment method with the invalid appointment
        var result = await _controller.CreateAppointment(invalidAppointment);

        // Assert: Verify that a BadRequestObjectResult is returned with the correct error message
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("CaregiverId is required.", badRequestResult.Value);

        // Ensure the repository's CreateAsync method is never called
        _mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<Appointment>()), Times.Never);
    }

    [Fact]
    public async Task AddAppointment_ShouldReturnConflict_WhenAppointmentAlreadyExists()
    {
        // Arrange: Create an appointment that already exists
        var existingAppointment = new Appointment
        {
            PatientId = 1,
            CaregiverId = 2,
            DateTime = DateTime.UtcNow.AddDays(1),
            Status = AppointmentStatus.Scheduled
        };

        // Mock the repository to return a list containing the existing appointment
        _mockRepository.Setup(repo => repo.GetByCaregiverIdAsync(existingAppointment.CaregiverId))
                       .ReturnsAsync(new List<Appointment> { existingAppointment });

        // Act: Call the CreateAppointment method with the existing appointment
        var result = await _controller.CreateAppointment(existingAppointment);

        // Assert: Verify that a ConflictObjectResult is returned with the correct error message
        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.Equal("Appointment already exists for the selected caregiver and time.", conflictResult.Value);

        // Ensure the repository's CreateAsync method is never called
        _mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<Appointment>()), Times.Never);
    }
}
