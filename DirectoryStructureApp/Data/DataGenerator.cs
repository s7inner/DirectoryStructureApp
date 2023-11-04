using DirectoryStructureApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DirectoryStructureApp.Data
{
    public class DataGenerator
    {
        public static async Task InitCatalogAsync(IServiceProvider serviceProvider)
        {
            using (var context = new CatalogDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<CatalogDbContext>>()))
            {
                if (context.MyCatalogs.Any())
                {
                    return;   // DB has been seeded
                }

                var parentCatalog = new MyCatalog
                {
                    Name = "Creating Digital Images",
                    Children = new List<MyCatalog>
                {
                    new MyCatalog { Name = "Resources" },
                    new MyCatalog { Name = "Evidence" },
                    new MyCatalog { Name = "Graphic Products" }
                }
                };

                parentCatalog.Children[0].Children = new List<MyCatalog>
            {
                new MyCatalog { Name = "Primary Sources" },
                new MyCatalog { Name = "Secondary Sources" }
            };

                parentCatalog.Children[2].Children = new List<MyCatalog>
            {
                new MyCatalog { Name = "Process" },
                new MyCatalog { Name = "Final Product" }
            };

                context.MyCatalogs.Add(parentCatalog);

                foreach (var child in parentCatalog.Children)
                {
                    if (child.Children != null)
                    {
                        context.MyCatalogs.AddRange(child.Children);
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }


}
