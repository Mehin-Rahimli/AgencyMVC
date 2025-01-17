using Agency.Areas.admin.ViewModels;
using Agency.DAL;
using Agency.Models;
using Agency.Utilities.Enums;
using Agency.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Agency.Areas.admin.Controllers
{
    [Area("Admin")]
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProjectController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var projectVM=await _context.Projects
                .Include(p=>p.Category)
                .Where(p=>!p.IsDeleted)
                .Select(p=>new GetProjectVM
                {
                    Name = p.Name,
                    Id = p.Id,
                    CategoryName=p.Category.Name,
                    Image=p.Image
                })
                .ToListAsync();
            return View(projectVM);
        }

        public async Task<IActionResult> Create()
        {
            CreateProjectVM projectVM = new CreateProjectVM
            {
                Categories = await _context.Categories.ToListAsync()
            };
            return View(projectVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create(CreateProjectVM projectVM)
        {
            projectVM.Categories=await _context.Categories.ToListAsync();
            if(!ModelState.IsValid) return View(projectVM);

            if (!projectVM.Image.ValidateSize(FileSize.MB, 2))
            {
                ModelState.AddModelError("Image", "Image size must be less than 2 MB");
                return View(projectVM);
            }
            if (!projectVM.Image.ValidateType("image/"))
            {
                ModelState.AddModelError("Image", "Image type is incorrect");
                return View(projectVM); 
            }
            bool result=projectVM.Categories.Any(p=>p.Id==projectVM.CategoryId);
            if(!result)
            {
                ModelState.AddModelError((nameof(CreateProjectVM.CategoryId)), "Category does not exists");
                return View(projectVM);
            }

            string imagepath=await projectVM.Image.CreateFileAsync(_env.WebRootPath,"assets","img","portfolio");

            Project project = new()
            {
                Name = projectVM.Name,
                Image=imagepath,
                CategoryId=projectVM.CategoryId.Value
            };

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult>Update(int? id)
        {
            if (id < 1 || id == null) return BadRequest();
            Project project=await _context.Projects.FirstOrDefaultAsync(p=>p.Id==id);
            if (project == null) return NotFound();

            UpdateProjectVM projectVM = new UpdateProjectVM
            {
                Name = project.Name,

                CategoryId = project.CategoryId,
                Categories=await _context.Categories.ToListAsync(),
                ExistingImage = project.Image,
            };
            return View(projectVM);
        }

        [HttpPost]
        public async Task<IActionResult>Update(int? id,UpdateProjectVM projectVM)
        {
            if (id < 1 || id == null) return BadRequest();
            Project existed = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (existed == null) return NotFound();

            projectVM.Categories =await _context.Categories.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View(projectVM);
            }
            if (projectVM.Image != null)
            {
                if (!projectVM.Image.ValidateSize(FileSize.MB, 2))
                {
                    ModelState.AddModelError("Image", "Image size must be less than 2 MB");
                    return View(projectVM);
                }
                if (!projectVM.Image.ValidateType("image/"))
                {
                    ModelState.AddModelError("Image", "Image type is incorrect");
                    return View(projectVM);
                }
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "img", "portfolio");
                existed.Image = await projectVM.Image.CreateFileAsync(_env.WebRootPath, "assets", "img", "portfolio");
            }
            else
            {
                existed.Image = projectVM.ExistingImage;
            }
            if (existed.CategoryId!=projectVM.CategoryId)
            {
                bool result=projectVM.Categories.Any(c=>c.Id==existed.CategoryId);
                if (!result)
                {
                    return View(projectVM);
                }
            }
            existed.Name = projectVM.Name;
            existed.CategoryId=projectVM.CategoryId.Value;
          await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult>Delete(int id)
        {
            if (id < 1 || id == null) return BadRequest();
            Project project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (project == null) return NotFound();
            if(!string.IsNullOrEmpty(project.Image))
            {
                project.Image.DeleteFile(_env.WebRootPath, "assets", "img", "portfolio");

            }
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
 