using Microsoft.AspNetCore.Mvc;

namespace DirectoryStructureApp.Interfaces
{
    public interface IJsonFileService
    {
        Task ImportDataFromJsonFileAsync(IFormFile file);
        Task<IActionResult> SaveMyCatalogsToJsonFileAsync(string body);
    }

}
