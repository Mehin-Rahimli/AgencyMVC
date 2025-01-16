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

                Employees=await _context.Employees.Include(e => e.Major).OrderByDescending(x => x.Id).Take(3).ToListAsync(),
                Projects=await _context.Projects.Include(e => e.Category).OrderByDescending(x => x.Id).Take(3).ToListAsync(),
                //Employees = await _context.Employees
                //.Where(e => !e.IsDeleted)
                //.Select(e => new Employee
                //{
                //    Name = e.Name,
                //    Surname = e.Surname,
                //    Image = e.Image,
                //    Major = e.Major,
                //    FacebookLink = e.FacebookLink,
                //    TwitterLink = e.TwitterLink,
                //    LindekInLink = e.LindekInLink
                //}).ToListAsync(),

                //Projects=await _context.Projects
                //.Where(e => !e.IsDeleted)
                //.Select(p=>new Project
                //{
                //    Name=p.Name,
                //    Image=p.Image,
                //    Category = p.Category

                //}).ToListAsync()
            };
            return View(homeVM);
        }
    }
}
