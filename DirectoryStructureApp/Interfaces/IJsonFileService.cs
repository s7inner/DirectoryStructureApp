using Microsoft.AspNetCore.Mvc;

namespace DirectoryStructureApp.Interfaces
{
    public interface IJsonFileService
    {
        void ImportDataFromJsonFile(IFormFile file);
        Task<IActionResult> SaveMyCatalogsToJsonFileAsync(string body);
    }

}
