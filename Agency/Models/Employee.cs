namespace Agency.Models
{
    public class Employee:BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public Major Major { get; set; }
        public int MajorId { get; set; }
        public string Image { get; set; }
        public string? FacebookLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? LindekInLink { get; set; }
    }
}
