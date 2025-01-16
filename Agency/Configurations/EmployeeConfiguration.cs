using Agency.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agency.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {

            builder.Property(p => p.Name).IsRequired().HasColumnType("varchar(50)");
            builder.Property(p => p.Surname).IsRequired().HasColumnType("varchar(50)");
        }
    }
}
