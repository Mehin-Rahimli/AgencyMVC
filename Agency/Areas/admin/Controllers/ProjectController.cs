using Microsoft.AspNetCore.Mvc;

namespace Agency.Areas.admin.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
