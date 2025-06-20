using Deduct.Data;
using Deduct.Models;
using Deduct.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Deduct.Service.Implement
{
    public class DeductService(JPDbContext DbContext) : IDeductService
    {
        private readonly JPDbContext _DbContext = DbContext;

        private async Task<IEnumerable<TmpDeductModel>> GetAllTmpDeductsAsync(DeductFilterModel filter)
        {
            var query = from a in _DbContext.JobDeduct
                        join b in _DbContext.JobDetail on a.Jobbarcode equals b.JobBarcode into bGroup
                        from b in bGroup.DefaultIfEmpty()

                        join c in _DbContext.TempProfile on b.EmpCode equals c.EmpCode into cGroup
                        from c in cGroup.DefaultIfEmpty()

                        join d in _DbContext.CpriceSale on b.Barcode equals d.Barcode into dGroup
                        from d in dGroup.DefaultIfEmpty()

                        join f in _DbContext.JobKeep on new { JobBarcode = a.Jobbarcode, a.Num } equals new { f.JobBarcode, f.Num } into fGroup
                        from f in fGroup.DefaultIfEmpty()

                        where (string.IsNullOrEmpty(a.SendDoc))

                        select new { a, b, c, d, f };

            bool hasOrderNoFilter = !string.IsNullOrEmpty(filter.OrderNo);
            bool hasRevDateFilter = filter.RevDate.HasValue && filter.RevDate.Value != DateTime.MinValue;

            if (hasOrderNoFilter)
                query = query.Where(x => x.b != null && x.b.OrderNo == filter.OrderNo);

            if (hasRevDateFilter)
                query = query.Where(x => x.a.MDate.Date == filter.RevDate!.Value.Date);

            if (!hasOrderNoFilter && !hasRevDateFilter)
                query = query.Take(1000);

            var result = await query.Select(x => new TmpDeductModel
            {
                RunDeduct = x.a.RunDeduct,
                JobBarcode = x.a.Jobbarcode,
                DeductQty = x.a.DeductQty,
                DocNo = x.a.DocNo ?? string.Empty,
                Doc_No = x.a.DocNo ?? string.Empty,
                EmpCode = x.a.EmpCode,
                QcQty = x.f != null ? x.f.TtQty : 0,
                OrderNo = x.b != null ? x.b.OrderNo : string.Empty,
                ListNo = x.b != null ? x.b.ListNo : string.Empty,
                LotNo = x.b != null ? x.b.LotNo : string.Empty,
                Article = x.b != null ? x.b.Article : string.Empty,
                Name = x.c != null ? x.c.Name : string.Empty,
                Picture = x.d != null ? x.d.Picture : string.Empty,
                ChkS = false
            }).ToListAsync();

            return result;
        }

        public async Task<PagedListModel<TmpDeductModel>> GetPagedTmpDeductsAsync(int page, int pageSize,DeductFilterModel filter)
        {
            var query = await GetAllTmpDeductsAsync(filter);

            var totalCount = query.Count();

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var skip = (page - 1) * pageSize;

            var items = query
                .OrderBy(x => x.RunDeduct)
                .Skip(skip)
                .Take(pageSize)
                .ToList();

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
