using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace WebApplication1.Models
{
    public static class SeedData
    {
      
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new WebApplication1Context(
                serviceProvider.GetRequiredService<DbContextOptions<WebApplication1Context>>()))
            {
         
                // Look for any scans
                if (context.Scan.Any())
                {
                    return;   // DB has been seeded
                }
              
            }
        }
    }
}
        
