using ApplicationTracker.Models;
using Microsoft.AspNetCore.Mvc;
using ApplicationTracker.Services;
using System.Collections.Generic;
using ApplicationTracker.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ApplicationTracker.Controllers
{
    /// <summary>
    /// API controller that exposes CRUD endpoints for job applications.
    /// Each user can only access their own applications
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _service;
        public ApplicationController(IApplicationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets the logged-in user's id from JWT token
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("User ID not found in token.");
        }

        /// <summary>
        /// Creates a new job application for the logged in user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateApplicationRequest request)
        {
            var userId = GetUserId();

            var application = new ApplicationModel
            {
                CompanyName = request.CompanyName,
                DateApplied = request.DateApplied,
                Status = request.Status,
                Notes = request.Notes,
                UserId = userId,
            };

            var created = await _service.Create(application);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Returns the logged-in user's applications with optional filters and pagination.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PagedResult<ApplicationModel>>> GetAll(
            [FromQuery] string? status,
            [FromQuery] string? company,
            [FromQuery] DateOnly? fromDate,
            [FromQuery] DateOnly? toDate,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var userId = GetUserId();

            var result = await _service.Search(userId, status, company, fromDate, toDate, page, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a job application by its Id for the logged-in user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApplicationModel>> GetById(int id)
        {
            var userId = GetUserId();

            var app = await _service.GetById(id, userId);

            // If not found return a 404
            if (app == null)
                return NotFound();

            return Ok(app);
        }

        /// <summary>
        /// Updates an existing application for the logged-in user.
        /// Returns 404 if the application does not exist.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateApplicationRequest request)
        {
            var userId = GetUserId();
            var updatedApplication = new ApplicationModel
            {
                CompanyName = request.CompanyName,
                DateApplied = request.DateApplied,
                Status = request.Status,
                Notes = request.Notes
            };

            var updated = await _service.Update(id, updatedApplication, userId);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Deletes a job application by id for the logged-in user.
        /// Returns 404 if the id does not exist.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            var deleted = await _service.Delete(id, userId);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
