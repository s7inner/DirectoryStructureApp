using DirectoryStructureApp.Data;
using DirectoryStructureApp.Interfaces;
using DirectoryStructureApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DirectoryStructureApp.Repositories
{
    public class MyCatalogRepository: IMyCatalogRepository
    {
        private readonly CatalogDbContext _context;

        public MyCatalogRepository(CatalogDbContext context)
        {
            _context = context;
        }


        public async Task<List<MyCatalog>> GetChildrenByIdAsync(int parentId)
        {
            var children = await _context.MyCatalogs.Where(c => c.MyCatalogId == parentId).ToListAsync();
            return children;
        }

        /*
        public async Task<IEnumerable<MyCatalog>> GetParentAndChildrenCatalogsAsync()
        {
            var parentCatalogs = await _context.MyCatalogs.Where(c => c.MyCatalogId == null).ToListAsync();
            var allCatalogs = await _context.MyCatalogs.ToListAsync();

            foreach (var parentCatalog in parentCatalogs)
            {
                parentCatalog.Children = allCatalogs.Where(c => c.MyCatalogId == parentCatalog.Id).ToList();
            }

            return parentCatalogs;
        }
        */


        //------------------------------
        /*
        public async Task<List<MyCatalog>> GetAllMyCatalogsAsync()
        {
            return await _context.MyCatalogs.ToListAsync();
        }

        public async Task<MyCatalog> GetMyCatalogByIdAsync(int id)
        {
            return await _context.MyCatalogs.FindAsync(id);
        }

        public async Task AddMyCatalogAsync(MyCatalog myCatalog)
        {
            _context.MyCatalogs.Add(myCatalog);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMyCatalogAsync(MyCatalog myCatalog)
        {
            _context.MyCatalogs.Update(myCatalog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMyCatalogAsync(MyCatalog myCatalog)
        {
            _context.MyCatalogs.Remove(myCatalog);
            await _context.SaveChangesAsync();
        }

        public bool MyCatalogExists(int id)
        {
            return _context.MyCatalogs.Any(e => e.Id == id);
        }
        */
    }
}
