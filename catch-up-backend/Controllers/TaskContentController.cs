using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    public class TaskContentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
