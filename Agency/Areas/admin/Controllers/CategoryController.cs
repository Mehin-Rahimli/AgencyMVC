using Agency.Areas.admin.ViewModels;
using Agency.DAL;
using Agency.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agency.Areas.admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var categoryVM = await _context.Categories
                .Where(c=>!c.IsDeleted)
                .Include(c=>c.Projects)
                .Select(c=>new GetCategoryVM
            {
                    Name = c.Name,
                    Id = c.Id,
                    ProjectCount=c.Projects.Count

            }).ToListAsync(); 


            return View(categoryVM);
        }

        public async Task<IActionResult> Create()
        {
            CreateCategoryVM categoryVM=new CreateCategoryVM();
            return View(categoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryVM categoryVM)
        {
            if (!ModelState.IsValid) return View();
            bool result=await _context.Categories.AnyAsync(c=>c.Name.Trim()==categoryVM.Name.Trim());
            if(result)
            {
                ModelState.AddModelError("Name", "Category name already exists");
                return RedirectToAction("Index");
            }

            Category category = new()
            {
                Name = categoryVM.Name
            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult>Update(int? id)
        {
            if(id<1 || id==null) return View();
            Category category = await _context.Categories.FirstOrDefaultAsync(c=>c.Id==id); 
            if (category==null) return NotFound();
            UpdateCategoryVM categoryVM = new UpdateCategoryVM
            {
                Name = category.Name,
                Id = category.Id

            };
            return View(categoryVM);
        }

        [HttpPost]
        public async Task<IActionResult>Update(int? id,UpdateCategoryVM categoryVM)
        {
            if (id < 1 || id == null) return BadRequest();
            Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result=await _context.Categories.AnyAsync(c=>c.Id!=id && c.Name.Trim()==categoryVM.Name.Trim());

            if (result)
            {
                ModelState.AddModelError("Name", "Name already exists");
                return View(categoryVM);
            }
            existed.Name= categoryVM.Name;

            _context.Categories.Update(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1 || id == null) return View();
            Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            existed.IsDeleted= true;
            _context.Categories.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
