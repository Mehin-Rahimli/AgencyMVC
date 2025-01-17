using Agency.DAL;
using Agency.Models;
using Agency.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agency.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM
            {

                Employees=await _context.Employees.Include(e => e.Major).OrderByDescending(x => x.Id).Take(10).ToListAsync(),
                Projects=await _context.Projects.Include(e => e.Category).OrderByDescending(x => x.Id).Take(10).ToListAsync(),
              

            };
            return View(homeVM);
        }
    }
}
