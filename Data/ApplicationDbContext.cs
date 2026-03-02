using Microsoft.EntityFrameworkCore;
using ApplicationTracker.Models;

namespace ApplicationTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        {

        }

        public DbSet<ApplicationModel> Applications { get; set; }
    }
}
