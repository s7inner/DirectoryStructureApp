using Microsoft.AspNetCore.Mvc;

namespace DirectoryStructureApp.Interfaces
{
    public interface IJsonFileService
    {
        void ImportDataFromJsonFile(IFormFile file);
        Task<IActionResult> SaveMyCatalogsToJsonFile(string body);
    }

}
