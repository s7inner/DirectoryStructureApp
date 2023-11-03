using DirectoryStructureApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DirectoryStructureApp.Data
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions options) : base(options) { }
        public DbSet<MyCatalog> MyCatalogs { get; set; }
    }
}
