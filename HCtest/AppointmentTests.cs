using HealthCareABApi.Controllers;
using HealthCareABApi.Models;
using HealthCareABApi.Repositories;
using HealthCareABApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HCtest;

public class AppointmentTests
{
    private readonly Mock<IAppointmentRepository> _mockRepository;
    private readonly Mock<AppointmentService> _mockService;
    private readonly AppointmentsController _controller;

    public AppointmentTests()
    {
        _mockRepository = new Mock<IAppointmentRepository>();
        _mockService = new Mock<AppointmentService>(_mockRepository.Object);
        _controller = new AppointmentsController(_mockService.Object);
    }
    [Fact]
    public async Task AddAppointment_ShouldReturnCreated_WhenValidData()
    {
        // Arrange
        var newAppointment = new Appointment
        {
            PatientId = 2,
            CaregiverId = 1,
            DateTime = DateTime.UtcNow.AddDays(1),
            Status = AppointmentStatus.Scheduled
        };

        // Mock service method to return empty list without throwing exception
        var mockService = new Mock<AppointmentService>(_mockRepository.Object) { CallBase = true };
        mockService.Setup(s => s.GetByCaregiverIdAsync(newAppointment.CaregiverId))
                   .ReturnsAsync(new List<Appointment>());

        var controller = new AppointmentsController(mockService.Object);

        _mockRepository.Setup(repo => repo.CreateAsync(newAppointment)).Returns(Task.CompletedTask);
        _mockRepository.Setup(repo => repo.GetByCaregiverIdAsync(newAppointment.CaregiverId))
                       .ReturnsAsync(new List<Appointment>());

        // Act
        var result = await controller.CreateAppointment(newAppointment);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(AppointmentsController.GetAppointment), createdAtActionResult.ActionName);
        var createdAppointment = Assert.IsType<Appointment>(createdAtActionResult.Value);
        Assert.Equal(newAppointment.PatientId, createdAppointment.PatientId);
        Assert.Equal(newAppointment.CaregiverId, createdAppointment.CaregiverId);
        _mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<Appointment>()), Times.Once);
    }
    [Fact]
    public async Task AddAppointment_ShouldReturnBadRequest_WhenInvalidData()
    {
        // Arrange
        Appointment invalidAppointment = null;

        // Act
        var result = await _controller.CreateAppointment(invalidAppointment);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid appointment data.", badRequestResult.Value);
        _mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<Appointment>()), Times.Never);
    }
    [Fact]
    public async Task AddAppointment_ShouldReturnBadRequest_WhenMissingCaregiverId()
    {
        // Arrange
        var invalidAppointment = new Appointment
        {
            PatientId = 1,
            CaregiverId = 0, // Ogiltigt värde
            DateTime = DateTime.UtcNow.AddDays(1),
            Status = AppointmentStatus.Scheduled
        };

        // Act
        var result = await _controller.CreateAppointment(invalidAppointment);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("CaregiverId is required.", badRequestResult.Value);
        _mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<Appointment>()), Times.Never);
    }
    [Fact]
    public async Task AddAppointment_ShouldReturnConflict_WhenAppointmentAlreadyExists()
    {
        // Arrange
        var existingAppointment = new Appointment
        {
            PatientId = 1,
            CaregiverId = 2,
            DateTime = DateTime.UtcNow.AddDays(1),
            Status = AppointmentStatus.Scheduled
        };

        _mockRepository.Setup(repo => repo.GetByCaregiverIdAsync(existingAppointment.CaregiverId))
                       .ReturnsAsync(new List<Appointment> { existingAppointment });

        // Act
        var result = await _controller.CreateAppointment(existingAppointment);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.Equal("Appointment already exists for the selected caregiver and time.", conflictResult.Value);
        _mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<Appointment>()), Times.Never);
    }

}
