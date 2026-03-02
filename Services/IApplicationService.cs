using ApplicationTracker.Dtos;
using ApplicationTracker.Models;

namespace ApplicationTracker.Services
{
    public interface IApplicationService
    {
        Task<ApplicationModel> Create(ApplicationModel application);
        Task<List<ApplicationModel>> GetAll();
        /// <summary>
        /// Allows filtering for the GetAll
        /// </summary>
        /// <param name="status"></param>
        /// <param name="company"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagedResult<ApplicationModel>> Search(
            string? status,
            string? company,
            DateOnly? fromDate,
            DateOnly? toDate,
            int page,
            int pageSize);
        Task<ApplicationModel?> GetById(int id);
        Task<bool> Update(int id, ApplicationModel updatedApplication);
        Task<bool> Delete(int id);
    }
}
