using System;
using HealthCareABApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareABApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProtectedController : ControllerBase
    {
        // This endpoint is protected and accessible only to users with the "User" role.
        // The [Authorize(Roles = Roles.User)] attribute restricts access to users with this role.
        [Authorize(Roles = Roles.User)]
        [HttpGet("user")]
        public IActionResult GetUserContent()
        {
            // Return protected content for users.
            return Ok("This is protected content for users.");
        }

        // This endpoint is protected and accessible only to users with the "Admin" role.
        // The [Authorize(Roles = Roles.Admin)] attribute restricts access to users with this role.
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("admin")]
        public IActionResult GetAdminContent()
        {
            // Return protected content for admins.
            return Ok("This is protected content for admins.");
        }
    }
}