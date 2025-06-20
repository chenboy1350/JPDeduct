using Deduct.Models;
using Deduct.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Deduct.Controllers
{
    public class BillsController(IHDeductService hDeduct) : Controller
    {
        private readonly IHDeductService _hDeduct = hDeduct;

        public async Task<IActionResult> Index()
        {
            var filter = new HDeductFilterModel
            {
                OrderNo = null,
                SendDate = null,
                Times = null
            };

            ViewBag.HDeductList = await _hDeduct.GetAllHDeductAsync();

            return View();
        }
    }
}
