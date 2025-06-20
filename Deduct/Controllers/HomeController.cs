using Deduct.Models;
using Deduct.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Deduct.Controllers
{
    public class HomeController(IDeductService deductService) : Controller
    {
        private readonly IDeductService _deductService = deductService;

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            PagedListModel<TmpDeductModel> model = await _deductService.GetPagedTmpDeductsAsync(page, pageSize, new DeductFilterModel());

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = model.TotalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = model.TotalCount;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SearchData(string orderNo = "", DateTime? revDate = null, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 5) pageSize = 5;
            if (pageSize > 100) pageSize = 100;

            DeductFilterModel filter = new()
            {
                OrderNo = orderNo ?? "",
                RevDate = revDate,
            };

            PagedListModel<TmpDeductModel> model = await _deductService.GetPagedTmpDeductsAsync(page, pageSize, filter);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = model.TotalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = model.TotalCount;

            return Json(new
            {
                success = true,
                html = await RenderPartialViewAsync("_DeductListPartial", model.Items),
                pagination = await RenderPartialViewAsync("_PaginationPartial", model),
                totalCount = model.TotalCount,
                currentPage = page,
                totalPages = model.TotalPages,
                pageSize
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetCheckedItems(string orderNo = "", DateTime? revDate = null, List<string>? checkedIds = null)
        {
            try
            {
                if (checkedIds == null || checkedIds.Count == 0)
                {
                    return Json(new { success = true, items = new List<object>() });
                }

                DeductFilterModel filter = new()
                {
                    OrderNo = orderNo ?? "",
                    RevDate = revDate,
                };

                PagedListModel<TmpDeductModel> allData = await _deductService.GetPagedTmpDeductsAsync(1, int.MaxValue, filter);

                List<TmpDeductModel> checkedItems = [];

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
                    html = await RenderPartialViewAsync("_SelectedDeductListPartial", checkedItems)
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private async Task<string> RenderPartialViewAsync(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            ViewData.Model = model;

            using var writer = new StringWriter();
            Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine? viewEngine = HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine)) as Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine ?? throw new InvalidOperationException("View engine not found.");
            var viewResult = viewEngine.FindView(ControllerContext, viewName, false);

            if (viewResult.View == null)
            {
                throw new InvalidOperationException($"View '{viewName}' not found.");
            }

            var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, writer, new Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelperOptions());

            await viewResult.View.RenderAsync(viewContext);
            return writer.GetStringBuilder().ToString();
        }
    }
}