using Microsoft.AspNetCore.Mvc;

namespace SampleMVCApp.Controllers
{
    public class HelloController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "Hello, this is sample message!";
            return View();
        }
    }
}
