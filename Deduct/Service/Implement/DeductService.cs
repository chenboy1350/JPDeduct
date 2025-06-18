using Deduct.Data;
using Deduct.Models;
using Deduct.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Deduct.Service.Implement
{
    public class DeductService(PrincessrDbContext DbContext) : IDeductService
    {
        private readonly PrincessrDbContext _DbContext = DbContext;

        private async Task<IEnumerable<TmpDeductModel>> GetAllTmpDeductsAsync()
        {
            var query = from a in _DbContext.JobDeduct
                        join b in _DbContext.JobDetail on a.Jobbarcode equals b.JobBarcode into bGroup
                        from b in bGroup.DefaultIfEmpty()

                        join c in _DbContext.TEmpProfile on b.EmpCode equals c.EmpCode into cGroup
                        from c in cGroup.DefaultIfEmpty()

                        join d in _DbContext.CPriceSale on b.Barcode equals d.Barcode into dGroup
                        from d in dGroup.DefaultIfEmpty()

                        join f in _DbContext.JobKeep on new { JobBarcode = a.Jobbarcode, a.Num } equals new { f.JobBarcode, f.Num } into fGroup
                        from f in fGroup.DefaultIfEmpty()

                            //where string.IsNullOrEmpty(a.SendDoc)

                        select new TmpDeductModel
                        {
                            RunDeduct = a.RunDeduct,
                            JobBarcode = a.Jobbarcode,
                            DeductQty = a.DeductQty,
                            DocNo = a.DocNo ?? string.Empty,
                            Doc_No = a.DocNo ?? string.Empty,
                            EmpCode = a.EmpCode,
                            QcQty = f != null ? f.TtQty : 0,
                            OrderNo = b != null ? b.OrderNo : string.Empty,
                            ListNo = b != null ? b.ListNo : string.Empty,
                            LotNo = b != null ? b.LotNo : string.Empty,
                            Article = b != null ? b.Article : string.Empty,
                            Name = c != null ? c.Name : string.Empty,
                            Picture = d != null ? d.Picture : string.Empty,
                            ChkS = false
                        };

            return await query.ToListAsync();
        }

        public async Task<PagedListModel<TmpDeductModel>> GetPagedTmpDeductsAsync(int page, int pageSize,DeductFilterModel filter)
        {
            var query = await GetAllTmpDeductsAsync();

            var totalCount = query.Count();

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var skip = (page - 1) * pageSize;

            var items = query
                .OrderBy(x => x.RunDeduct)
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.OrderNo))
                {
                    items = [.. items.Where(x => x.OrderNo.Contains(filter.OrderNo, StringComparison.OrdinalIgnoreCase))];
                }
            }

                return new PagedListModel<TmpDeductModel>
            {
                Items = items,
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                DeductFilter = filter ?? new DeductFilterModel()
            };
        }
    }
}
