using Agency.Models;

namespace Agency.Areas.admin.ViewModels
{
    public class CreateProjectVM
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public ICollection<Category>? Categories { get; set; }
        public int CategoryId { get; set; }
    }
}
