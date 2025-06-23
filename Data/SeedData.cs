using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using state_software_marketplace.Models;
using System;
using System.Linq;

namespace state_software_marketplace.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any products.
                if (context.SoftwareProducts.Any())
                {
                    return;   // DB has been seeded
                }

                context.SoftwareProducts.AddRange(
                    new SoftwareProduct
                    {
                        Name = "OpenGov Budgeting",
                        Description = "Cloud-based budgeting and performance management software for governments.",
                        Vendor = "OpenGov",
                        Category = "Budgeting"
                    },
                    new SoftwareProduct
                    {
                        Name = "Tyler Munis",
                        Description = "ERP solution for the public sector, including financials, HR, and more.",
                        Vendor = "Tyler Technologies",
                        Category = "ERP"
                    },
                    new SoftwareProduct
                    {
                        Name = "Granicus govDelivery",
                        Description = "Mass communication platform for government outreach.",
                        Vendor = "Granicus",
                        Category = "Communications"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
