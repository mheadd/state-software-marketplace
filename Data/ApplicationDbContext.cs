using Microsoft.EntityFrameworkCore;
using state_software_marketplace.Models;

namespace state_software_marketplace.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SoftwareProduct> SoftwareProducts { get; set; }
    }
}
