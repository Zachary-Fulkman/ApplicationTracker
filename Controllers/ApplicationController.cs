using ApplicationTracker.Models;
using Microsoft.AspNetCore.Mvc;
using ApplicationTracker.Services;
using System.Collections.Generic;

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
        /// <param name="application"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(ApplicationModel application)
        {
            var createdApplication = _service.Create(application);
            // Retruns 201 created and includes location
            return CreatedAtAction(nameof(GetById), new { id = createdApplication.Id }, createdApplication);
        }

        /// <summary>
        /// Returns all saved applications
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<ApplicationModel>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        /// <summary>
        /// Retrieves a job application by its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public ActionResult<ApplicationModel> GetById(int id)
        {
            // Finds application in the List
            var app = _service.GetById(id);

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
        public IActionResult Update(int id, ApplicationModel application)
        {
            var updated = _service.Update(id, application);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Deletes a job application by id.
        /// Returns 404 if the id does not exist.
        /// </summary>
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var deleted = _service.Delete(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
