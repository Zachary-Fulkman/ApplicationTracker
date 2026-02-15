using ApplicationTracker.Models;

namespace ApplicationTracker.Services
{
    public interface IApplicationService
    {
        ApplicationModel Create(ApplicationModel application);
        List<ApplicationModel> GetAll();
        ApplicationModel? GetById(int id);
        bool Update(int id, ApplicationModel updatedApplication);
        bool Delete(int id);
    }
}
