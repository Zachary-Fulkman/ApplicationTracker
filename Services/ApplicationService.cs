using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ApplicationTracker.Models;

namespace ApplicationTracker.Services
{
    /// <summary>
    /// In-memory implementation of application CRUD operations.
    /// Acts as a temporary data store while the application is running.
    /// Data is lost when the application restarts.
    /// </summary>
    public class ApplicationService : IApplicationService
    {
        private readonly List<ApplicationModel> _applications = new();
        private int _idCounter = 0;

        /// <summary>
        /// Creates a new application, assigns a unique Id,
        /// and stores it in memory.
        /// </summary>
        public ApplicationModel Create(ApplicationModel application)
        {
            application.Id = Interlocked.Increment(ref _idCounter);
            _applications.Add(application);
            return application;
        }

        /// <summary>
        /// Returns all stored applications.
        /// </summary>
        public List<ApplicationModel> GetAll()
        {
            return _applications.ToList();
        }

        /// <summary>
        /// Retrieves a single application by Id.
        /// Returns null if not found.
        /// </summary>
        public ApplicationModel? GetById(int id)
        {
            return _applications.FirstOrDefault(a => a.Id == id);
        }

        /// <summary>
        /// Updates an existing application.
        /// Returns true if the application was found and updated,
        /// false if no matching Id exists.
        /// </summary>
        public bool Update(int id, ApplicationModel updatedApplication)
        {
            var existing = _applications.FirstOrDefault(a => a.Id == id);
            if (existing == null)
                return false;
            
            existing.CompanyName = updatedApplication.CompanyName;
            existing.DateApplied = updatedApplication.DateApplied;
            existing.Status = updatedApplication.Status;
            existing.Notes = updatedApplication.Notes;
            return true;
        }

        /// <summary>
        /// Deletes an application by Id.
        /// Returns true if deletion occurred,
        /// false if the Id was not found.
        /// </summary>
        public bool Delete(int id)
        {
            var itemToDelete = _applications.FirstOrDefault(a => a.Id == id);
            if (itemToDelete == null)
                return false;
            _applications.Remove(itemToDelete);
            return true;
        }
    }
}
