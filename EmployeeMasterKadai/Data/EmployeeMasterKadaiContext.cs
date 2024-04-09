using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmployeeMasterKadai.Models;

namespace EmployeeMasterKadai.Data
{
    public class EmployeeMasterKadaiContext : DbContext
    {
        public EmployeeMasterKadaiContext (DbContextOptions<EmployeeMasterKadaiContext> options)
            : base(options)
        {
        }

        public DbSet<EmployeeMasterKadai.Models.Employee> Employees { get; set; } = default!;
        public DbSet<EmployeeMasterKadai.Models.Schedule> Schedules { get; set; } = default!;
    }
}
