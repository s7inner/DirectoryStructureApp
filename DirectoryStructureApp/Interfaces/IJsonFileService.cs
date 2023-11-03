namespace DirectoryStructureApp.Interfaces
{
    public interface IJsonFileService
    {
        void ImportDataFromJsonFile(IFormFile file);
        Task SaveMyCatalogsToJsonFile(string filePath);
    }

}
