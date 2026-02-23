using ApplicationTracker.Models;

namespace ApplicationTracker.Services
{
    public interface IApplicationService
    {
        Task<ApplicationModel> Create(ApplicationModel application);
        Task<List<ApplicationModel>> GetAll();
        Task<ApplicationModel?> GetById(int id);
        Task<bool> Update(int id, ApplicationModel updatedApplication);
        Task<bool> Delete(int id);
    }
}
