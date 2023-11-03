using DirectoryStructureApp.Models;

namespace DirectoryStructureApp.Interfaces
{
    public interface IMyCatalogRepository
    {
        List<MyCatalog> GetAll();
        void AddListCatalogs(List<MyCatalog> myCatalog);
        void DeleteAllMyCatalogs();
    }
}
