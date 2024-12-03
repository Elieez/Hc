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

namespace HCtest
{
    public class AvailabilityControllerTests
    {
        private readonly Mock<IAvailabilityRepository> _mockRepository;
        private readonly AvailabilityController _controller;
        public AvailabilityControllerTests()
        {
            // Mock repository
            _mockRepository = new Mock<IAvailabilityRepository>();
            // Instantiate controller with mocked repository
            _controller = new AvailabilityController(_mockRepository.Object);
        }
        [Fact]
        public async Task AddAvailability_ShouldReturnCreated_WhenValidData()
        {
            // Arrange
            var newAvailability = new Availability
            {
                CaregiverId = 101,
                AvailableSlots = new List<AvailableSlot>
                {
                    new AvailableSlot { Date = DateTime.UtcNow },
                    new AvailableSlot { Date = DateTime.UtcNow.AddHours(1) }
                }
            };
            _mockRepository.Setup(repo => repo.AddAvailabilityAsync(newAvailability)).Returns(Task.CompletedTask);
            // Act
            var result = await _controller.AddAvailability(newAvailability);
            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetAvailabilityById", createdAtActionResult.ActionName);
            var createdAvailability = Assert.IsType<Availability>(createdAtActionResult.Value);
            Assert.Equal(newAvailability.CaregiverId, createdAvailability.CaregiverId);
            _mockRepository.Verify(repo => repo.AddAvailabilityAsync(It.IsAny<Availability>()), Times.Once);
        }
        [Fact]
        public async Task AddAvailability_ShouldReturnBadRequest_WhenInvalidData()
        {
            // Arrange
            Availability invalidAvailability = null; // Simulate null input
            // Act
            var result = await _controller.AddAvailability(invalidAvailability);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid availability data.", badRequestResult.Value);
            _mockRepository.Verify(repo => repo.AddAvailabilityAsync(It.IsAny<Availability>()), Times.Never);
        }
    }
}