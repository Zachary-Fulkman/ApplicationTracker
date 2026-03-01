using ApplicationTracker.Models;
using Microsoft.AspNetCore.Mvc;
using ApplicationTracker.Services;
using System.Collections.Generic;
using ApplicationTracker.Dtos;

namespace ApplicationTracker.Controllers
{
    /// <summary>
    /// API controller that exposes CRUD endpoints for job applications.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _service;
        public ApplicationController(IApplicationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates a new job application.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateApplicationRequest request)
        {
            var application = new ApplicationModel
            {
                CompanyName = request.CompanyName,
                DateApplied = request.DateApplied,
                Status = request.Status,
                Notes = request.Notes
            };

            var created = await _service.Create(application);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Returns applications with optional filters and pagination.
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
            var result = await _service.Search(status, company, fromDate, toDate, page, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a job application by its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApplicationModel>> GetById(int id)
        {
            var app = await _service.GetById(id);

            // If not found return a 404
            if (app == null)
                return NotFound();

            return Ok(app);
        }

        /// <summary>
        /// Updates an existing application using the id from the route.
        /// Returns 404 if the application does not exist.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateApplicationRequest request)
        {
            var updatedApplication = new ApplicationModel
            {
                CompanyName = request.CompanyName,
                DateApplied = request.DateApplied,
                Status = request.Status,
                Notes = request.Notes
            };

            var updated = await _service.Update(id, updatedApplication);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Deletes a job application by id.
        /// Returns 404 if the id does not exist.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.Delete(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
