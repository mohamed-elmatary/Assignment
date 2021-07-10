using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project1.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Models.MainContext
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MainDbContext())
            {
                // Look for any movies.
                if (context.Steps.Any())
                {
                    return;   // DB has been seeded
                }

                context.Steps.AddRange(
                     new Step
                     {
                         Name = "Step 1"
                     },

                     new Step
                     {
                         Name = "Step 2"
                     },

                     new Step
                     {
                         Name = "Step 3"
                     }

                );
                context.SaveChanges();
            }
        }
    }
}
