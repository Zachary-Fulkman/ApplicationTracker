using ApplicationTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace ApplicationTracker.Controllers
{
    /// <summary>
    /// Controller that handles CRUD for the application data from the ApplicationModel class
    /// Temporary in-memory storage
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    public class ApplicationController : ControllerBase
    {
        /// <summary>
        /// List acts as our temporary database
        /// </summary>
        private static readonly List<ApplicationModel> _applications = new();

        /// <summary>
        /// Counter used to assign ID
        /// Static so it increments across requests
        /// </summary>
        private static int _idCounter = 0;

        /// <summary>
        /// New job application creation
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(ApplicationModel application)
        {
            // Server assigns ID
            application.Id = Interlocked.Increment(ref _idCounter);
            // Stores the application in the List
            _applications.Add(application);
            // Retruns 201 created and includes location
            return CreatedAtAction(nameof(GetById), new { id = application.Id }, application);
        }

        /// <summary>
        /// Returns all saved applications
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<ApplicationModel>> GetAll()
        {
            return _applications;
        }

        /// <summary>
        /// Returns an application by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public ActionResult<ApplicationModel> GetById(int id)
        {
            // Finds application in the List
            var app = _applications.FirstOrDefault(a => a.Id == id);

            // If not found return a 404
            if (app == null)
                return NotFound();

            return app;
        }

        /// <summary>
        /// Updates an existing application using the id from the route.
        /// Returns 404 if the application does not exist.
        /// </summary>
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, ApplicationModel application)
        {
            var foundId = _applications.FirstOrDefault(a => a.Id == id);
            if (foundId == null)
                return NotFound();

            // Apply updated values to the stored object
            foundId.CompanyName = application.CompanyName;
            foundId.DateApplied = application.DateApplied;
            foundId.Status = application.Status;
            foundId.Notes = application.Notes;

            return NoContent();

        }

        /// <summary>
        /// Deletes an application by id from the in-memory collection.
        /// Returns 404 if the application does not exist.
        /// </summary>
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var itemToDelete = _applications.FirstOrDefault(a => a.Id == id);
            if (itemToDelete == null)
            {
                return NotFound();
            }
            _applications.Remove(itemToDelete);
            return NoContent();
        }
    }
}
