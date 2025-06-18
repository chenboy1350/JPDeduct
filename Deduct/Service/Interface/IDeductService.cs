using Deduct.Models;

namespace Deduct.Service.Interface
{
    public interface IDeductService
    {
        Task<PagedListModel<TmpDeductModel>> GetPagedTmpDeductsAsync(int page, int pageSize, DeductFilterModel filter);
    }
}
