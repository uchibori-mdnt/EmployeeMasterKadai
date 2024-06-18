using Microsoft.EntityFrameworkCore;

namespace EmployeeMasterKadai.Data
{
    public class EmployeeMasterKadaiContext : DbContext
    {
        public EmployeeMasterKadaiContext(DbContextOptions<EmployeeMasterKadaiContext> options)
            : base(options)
        {
        }

        public DbSet<EmployeeMasterKadai.Models.Employee> Employees { get; set; } = default!;
        public DbSet<EmployeeMasterKadai.Models.Schedule> Schedules { get; set; } = default!;
    }
}
