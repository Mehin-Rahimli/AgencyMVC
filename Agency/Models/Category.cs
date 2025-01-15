namespace Agency.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Project>? Projects{ get; set; } 
    }
}
