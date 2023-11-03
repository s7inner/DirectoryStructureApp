using DirectoryStructureApp.Data;
using DirectoryStructureApp.Interfaces;
using DirectoryStructureApp.Models;

namespace DirectoryStructureApp.Repositories
{
    public class MyCatalogRepository: IMyCatalogRepository
    {
        private readonly CatalogDbContext _context;

        public MyCatalogRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public void AddListCatalogs(List<MyCatalog> catalogs)
        {
            foreach (var catalog in catalogs)
            {
                _context.MyCatalogs.Add(catalog);
            }

            _context.SaveChanges();
        }
        public List<MyCatalog> GetAll()
        {
            return _context.MyCatalogs.ToList();
        }

        public void DeleteAllMyCatalogs()
        {
            var allCatalogs = _context.MyCatalogs.ToList();
            _context.MyCatalogs.RemoveRange(allCatalogs);
            _context.SaveChanges();
        }
    }
}
