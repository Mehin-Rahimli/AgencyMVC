using Agency.Models;
using System.ComponentModel.DataAnnotations;

namespace Agency.Areas.admin.ViewModels
{
    public class CreateProjectVM
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public ICollection<Category>? Categories { get; set; }
        [Required]
        public int? CategoryId { get; set; }
    }
}
