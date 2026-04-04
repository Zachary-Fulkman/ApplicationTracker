using Microsoft.EntityFrameworkCore;
using ApplicationTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ApplicationTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        {

        }

        public DbSet<ApplicationModel> Applications { get; set; }
    }
}
