using ApplicationTracker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ApplicationTracker.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly List<ApplicationModel> _applications = new();
        private int _idCounter = 0;

        public ApplicationModel Create(ApplicationModel application)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationModel> GetAll()
        {
            return _applications;
        }

        public ApplicationModel? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, ApplicationModel updatedApplication)
        {
            throw new NotImplementedException();
        }
    }
}
