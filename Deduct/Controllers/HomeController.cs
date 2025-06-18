using Deduct.Models;
using Deduct.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Deduct.Controllers
{
    public class HomeController(IDeductService deductService) : Controller
    {
        private readonly IDeductService _deductService = deductService;

        public async Task<IActionResult> Index(string orderNo = "", DateTime? orderDate = null, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 5) pageSize = 5;
            if (pageSize > 100) pageSize = 100;

            DeductFilterModel filter = new()
            {
                OrderNo = orderNo ?? "",
                OrderDate = orderDate,
            };

            PagedListModel<TmpDeductModel> model = await _deductService.GetPagedTmpDeductsAsync(page, pageSize, filter);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = model.TotalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = model.TotalCount;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SearchData(string orderNo = "", DateTime? orderDate = null, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 5) pageSize = 5;
            if (pageSize > 100) pageSize = 100;

            DeductFilterModel filter = new()
            {
                OrderNo = orderNo ?? "",
                OrderDate = orderDate,
            };

            PagedListModel<TmpDeductModel> model = await _deductService.GetPagedTmpDeductsAsync(page, pageSize, filter);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = model.TotalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = model.TotalCount;

            return Json(new
            {
                success = true,
                html = await RenderPartialViewToString("_DeductListPartial", model.Items),
                pagination = await RenderPartialViewToString("_PaginationPartial", model),
                totalCount = model.TotalCount,
                currentPage = page,
                totalPages = model.TotalPages,
                pageSize = pageSize
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetCheckedItems(string orderNo = "", DateTime? orderDate = null, List<string> checkedIds = null)
        {
            try
            {
                if (checkedIds == null || !checkedIds.Any())
                {
                    return Json(new { success = true, items = new List<object>() });
                }

                // ใช้ filter เดียวกับที่ใช้ในการค้นหา
                DeductFilterModel filter = new()
                {
                    OrderNo = orderNo ?? "",
                    OrderDate = orderDate,
                };

                // ดึงข้อมูลทั้งหมดที่ตรงกับ filter ปัจจุบัน
                PagedListModel<TmpDeductModel> allData = await _deductService.GetPagedTmpDeductsAsync(1, int.MaxValue, filter);

                var checkedItems = new List<TmpDeductModel>();

                // กรอง items ที่ถูก checked
                foreach (var id in checkedIds)
                {
                    var item = allData.Items.FirstOrDefault(x =>
                        x.RunDeduct == Convert.ToInt32(id));

                    if (item != null)
                    {
                        checkedItems.Add(item);
                    }
                }

                return Json(new
                {
                    success = true,
                    items = checkedItems.Select(item => new
                    {
                        OrderNo = item.OrderNo ?? "",
                        ListNo = item.ListNo ?? "",
                        LotNo = item.LotNo ?? "",
                        Article = item.Article ?? "",
                        DeductQty = item.DeductQty.ToString() ?? "",
                        QcQty = item.QcQty.ToString() ?? "",
                        Doc_No = item.Doc_No ?? "",
                        DocNo = item.DocNo ?? "",
                        Name = item.Name ?? ""
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Search(PagedListModel<TmpDeductModel> filter)
        {
            filter ??= new PagedListModel<TmpDeductModel>();

            return RedirectToAction("Index", new
            {
                orderNo = filter.DeductFilter.OrderNo,
                orderDate = filter.DeductFilter.OrderDate?.ToString("yyyy-MM-dd"),
                page = 1,
                pageSize = 10
            });
        }

        private async Task<string> RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                var viewEngine = HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine)) as Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine;
                var viewContext = new ViewContext(ControllerContext, viewEngine.FindView(ControllerContext, viewName, false).View, ViewData, TempData, writer, new Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelperOptions());

                await viewEngine.FindView(ControllerContext, viewName, false).View.RenderAsync(viewContext);
                return writer.GetStringBuilder().ToString();
            }
        }
    }
}