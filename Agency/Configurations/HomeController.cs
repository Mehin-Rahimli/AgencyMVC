using Agency.DAL;
using Agency.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Agency.Configurations
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        //public async Task<IActionResult> Index()
        //{
        //    //HomeVM homeVM = new HomeVM
        //    //{
        //    //    Employees = await _context
        //    //};
        //    //return View();
        //}
    }
}
