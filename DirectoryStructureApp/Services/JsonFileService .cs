using DirectoryStructureApp.Interfaces;
using DirectoryStructureApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DirectoryStructureApp.Services
{
    public class JsonFileService : IJsonFileService
    {

        private readonly IMyCatalogRepository _myCatalogRepository;

        public JsonFileService(IMyCatalogRepository myCatalogRepository)
        {
            _myCatalogRepository = myCatalogRepository;
        }

        public async Task<IActionResult> SaveMyCatalogsToJsonFile(string body)
        {
            try
            {
                JObject jsonObject = JObject.Parse(body);
                string selectedDirectoryPath = jsonObject["selectedDirectoryPath"].ToString();

                if (string.IsNullOrEmpty(selectedDirectoryPath))
                {
                    return new BadRequestObjectResult("Шлях до директорії порожній.");
                }

                var myCatalogs = _myCatalogRepository.GetAll();
                var parentCatalogs = myCatalogs.Where(c => c.MyCatalogId == null).ToList();
                var result = parentCatalogs.Select(c => GetCatalogTree(c, myCatalogs)).ToList();
                var serializedCatalogs = JsonConvert.SerializeObject(result, Formatting.Indented);

                var path = Path.Combine(selectedDirectoryPath, "output.json");
                File.WriteAllText(path, serializedCatalogs);

                return new OkResult();
            }
            catch (Exception ex)
            {
                // Обробка будь-яких винятків, які можуть виникнути
                return new ObjectResult("Сталася помилка: " + ex.Message) { StatusCode = 500 };
            }
        }

        //Забирає поле Id в JSON файлі на вихід, оскільки при імпорті не потрібні
        private dynamic GetCatalogTree(MyCatalog catalog, List<MyCatalog> catalogs)
        {
            return new
            {
                catalog.Name,
                catalog.MyCatalogId,
                Children = catalogs.Where(c => c.MyCatalogId == catalog.Id).Select(c => GetCatalogTree(c, catalogs)).ToList()
            };
        }

        public void ImportDataFromJsonFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                // Обробка помилки, якщо файл не було вибрано
                throw new ArgumentNullException("File not selected");
            }

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var jsonString = reader.ReadToEnd();

                try
                {
                    // Десеріалізація JSON-даних в об'єкт або колекцію об'єктів
                    var catalogs = JsonConvert.DeserializeObject<List<MyCatalog>>(jsonString);

                    // Збереження отриманих даних в базі даних
                    _myCatalogRepository.AddListCatalogs(catalogs);
                }
                catch (Exception ex)
                {
                    // Обробка помилки десеріалізації або збереження в базі даних
                    throw new Exception($"Error: {ex.Message}");
                }
            }
        }
    }
}
