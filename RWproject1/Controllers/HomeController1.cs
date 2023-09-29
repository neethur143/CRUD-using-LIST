using Microsoft.AspNetCore.Mvc;

namespace RWproject1.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
