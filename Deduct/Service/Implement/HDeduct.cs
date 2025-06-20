using Deduct.Data;
using Deduct.Data.Entities;
using Deduct.Models;
using Deduct.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Deduct.Service.Implement
{
    public class HDeductService(JPDbContext DbContext) : IHDeductService
    {
        private readonly JPDbContext _DbContext = DbContext;

        public async Task<IEnumerable<JobHdeduct>> GetAllHDeductAsync()
        {
            return await _DbContext.JobHdeduct.OrderByDescending(x => x.SendDoc).Select(x => new JobHdeduct
            {
                SendDoc = x.SendDoc,
                Senddate = x.Senddate,
                Username = x.Username
            }).Take(1000).ToListAsync();
        }

        public async Task<PagedListModel<JobHdeduct>> GetPagedHeadDeductAsync(int page, int pageSize, HDeductFilterModel filter)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
    }
}
