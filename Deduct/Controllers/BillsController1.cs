using Microsoft.AspNetCore.Mvc;

namespace Deduct.Controllers
{
    public class BillsController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
