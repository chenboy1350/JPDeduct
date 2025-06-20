using Deduct.Data.Entities;
using Deduct.Models;

namespace Deduct.Service.Interface
{
    public interface IHDeductService
    {
        Task<IEnumerable<JobHdeduct>> GetAllHDeductAsync();
        Task<PagedListModel<JobHdeduct>> GetPagedHeadDeductAsync(int page, int pageSize, HDeductFilterModel filter);
    }
}
