using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiIdentity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApiIdentity.data
{
    public class HRContext:IdentityDbContext<ApplicationUser>
    {
        public DbSet<Employee> employees { get; set; }
        


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("data source=localhost; initial catalog=identityWebApisession; integrated security=true");
            base.OnConfiguring(optionsBuilder);
        }

    }
}
