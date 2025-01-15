using Agency.Models;

namespace Agency.ViewModels
{
    public class HomeVM
    {
        public ICollection<Employee>? Employees { get; set; }
        public ICollection<Project>? Projects { get; set; }      
    }
}
