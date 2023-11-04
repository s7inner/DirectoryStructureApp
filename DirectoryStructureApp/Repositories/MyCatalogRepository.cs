using DirectoryStructureApp.Data;
using DirectoryStructureApp.Interfaces;
using DirectoryStructureApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DirectoryStructureApp.Repositories
{
    public class MyCatalogRepository : IMyCatalogRepository
    {
        private readonly CatalogDbContext _context;

        public MyCatalogRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task AddListCatalogsAsync(List<MyCatalog> catalogs)
        {
            foreach (var catalog in catalogs)
            {
                _context.MyCatalogs.Add(catalog);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<MyCatalog>> GetAllAsync()
        {
            return await _context.MyCatalogs.ToListAsync();
        }

        public async Task DeleteAllMyCatalogsAsync()
        {
            var allCatalogs = await _context.MyCatalogs.ToListAsync();
            _context.MyCatalogs.RemoveRange(allCatalogs);
            await _context.SaveChangesAsync();
        }
    }
}
