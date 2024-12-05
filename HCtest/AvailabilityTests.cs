using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HealthCareABApi.Controllers;
using HealthCareABApi.Models;
using HealthCareABApi.Repositories;
using HealthCareABApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HCtest;

public class AvailabilityControllerTests
{
    private readonly Mock<IAvailabilityRepository> _mockRepository;
    private readonly AvailabilityController _controller;

    public AvailabilityControllerTests()
    {
        // Mock repository for simulating data access
        _mockRepository = new Mock<IAvailabilityRepository>();

        // Instantiate the controller with the mocked repository
        _controller = new AvailabilityController(_mockRepository.Object);
    }

    [Fact]
    public async Task AddAvailability_ShouldReturnCreated_WhenValidData()
    {
        // Arrange: Set up valid availability data and mock repository behavior
        var newAvailability = new Availability
        {
            CaregiverId = 101,
            AvailableSlots = new List<AvailableSlot>
            {
                new AvailableSlot { Date = DateTime.UtcNow },
                new AvailableSlot { Date = DateTime.UtcNow.AddHours(1) }
            }
        };

        // Simulate successful addition of availability
        _mockRepository.Setup(repo => repo.AddAvailabilityAsync(newAvailability)).Returns(Task.CompletedTask);

        // Act: Call the AddAvailability method
        var result = await _controller.AddAvailability(newAvailability);

        // Assert: Verify the result is a CreatedAtActionResult pointing to the correct action
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetAvailabilityById", createdAtActionResult.ActionName);

        // Ensure the returned availability matches the input data
        var createdAvailability = Assert.IsType<Availability>(createdAtActionResult.Value);
        Assert.Equal(newAvailability.CaregiverId, createdAvailability.CaregiverId);

        // Verify the repository's AddAvailabilityAsync method was called exactly once
        _mockRepository.Verify(repo => repo.AddAvailabilityAsync(It.IsAny<Availability>()), Times.Once);
    }

    [Fact]
    public async Task AddAvailability_ShouldReturnBadRequest_WhenInvalidData()
    {
        // Arrange: Set up invalid data (null in this case)
        Availability invalidAvailability = null;

        // Act: Call the AddAvailability method with invalid data
        var result = await _controller.AddAvailability(invalidAvailability);

        // Assert: Verify a BadRequestObjectResult is returned with the correct error message
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid availability data.", badRequestResult.Value);

        // Ensure the repository method was never called
        _mockRepository.Verify(repo => repo.AddAvailabilityAsync(It.IsAny<Availability>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAvailabilities_ShouldReturnOkWithList()
    {
        // Arrange: Create a mock list of availabilities and set up the repository to return it
        var availabilities = new List<Availability>
        {
            new Availability { Id = 1, CaregiverId = 101, AvailableSlots = new List<AvailableSlot> { new AvailableSlot { Date = DateTime.UtcNow } } },
            new Availability { Id = 2, CaregiverId = 102, AvailableSlots = new List<AvailableSlot> { new AvailableSlot { Date = DateTime.UtcNow.AddHours(1) } } }
        };

        // Simulate repository returning the list
        _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(availabilities);

        // Act: Call the GetAllAvailabilities method
        var result = await _controller.GetAllAvailabilities();

        // Assert: Verify the result is an OkObjectResult with the correct list of availabilities
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Availability>>(okResult.Value);
        Assert.Equal(availabilities.Count, returnValue.Count());

        // Ensure the repository's GetAllAsync method was called exactly once
        _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAvailabilitiesByCaregiverId_ShouldReturnOkWithList()
    {
        // Arrange: Set up mock data for a specific caregiver
        int caregiverId = 101;
        var availabilities = new List<Availability>
        {
            new Availability { Id = 1, CaregiverId = caregiverId, AvailableSlots = new List<AvailableSlot> { new AvailableSlot { Date = DateTime.UtcNow } } }
        };

        // Simulate repository returning the list for the caregiver
        _mockRepository.Setup(repo => repo.GetAvailabilitiesAsync(caregiverId)).ReturnsAsync(availabilities);

        // Act: Call the GetAvailabilitiesByCaregiverId method
        var result = await _controller.GetAvailabilitiesByCaregiverId(caregiverId);

        // Assert: Verify the result is an OkObjectResult with the correct list
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Availability>>(okResult.Value);
        Assert.Equal(availabilities.Count, returnValue.Count());
        Assert.All(returnValue, a => Assert.Equal(caregiverId, a.CaregiverId));

        // Ensure the repository's GetAvailabilitiesAsync method was called once
        _mockRepository.Verify(repo => repo.GetAvailabilitiesAsync(caregiverId), Times.Once);
    }

    [Fact]
    public async Task DeleteAvailability_ShouldReturnNoContent_WhenAvailabilityExists()
    {
        // Arrange: Set up mock availability and simulate repository methods
        int availabilityId = 1;
        var availability = new Availability { Id = availabilityId, CaregiverId = 101 };

        // Simulate getting the availability by ID and deleting it
        _mockRepository.Setup(repo => repo.GetByIdAsync(availabilityId)).ReturnsAsync(availability);
        _mockRepository.Setup(repo => repo.DeleteAvailabilityAsync(availabilityId)).Returns(Task.CompletedTask);

        // Act: Call the DeleteAvailability method
        var result = await _controller.DeleteAvailability(availabilityId);

        // Assert: Verify the result is a NoContentResult, indicating successful deletion
        Assert.IsType<NoContentResult>(result);

        // Ensure the repository's GetByIdAsync and DeleteAvailabilityAsync methods were called once each
        _mockRepository.Verify(repo => repo.GetByIdAsync(availabilityId), Times.Once);
        _mockRepository.Verify(repo => repo.DeleteAvailabilityAsync(availabilityId), Times.Once);
    }
}
