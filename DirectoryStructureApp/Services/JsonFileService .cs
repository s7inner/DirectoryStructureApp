using DirectoryStructureApp.Interfaces;
using DirectoryStructureApp.Models;
using Newtonsoft.Json;

namespace DirectoryStructureApp.Services
{
    public class JsonFileService : IJsonFileService
    {

        private readonly IMyCatalogRepository _myCatalogRepository;

        public JsonFileService(IMyCatalogRepository myCatalogRepository)
        {
            _myCatalogRepository = myCatalogRepository;
        }

        public async Task SaveMyCatalogsToJsonFile(string fileName)
        {
            var myCatalogs = _myCatalogRepository.GetAll();

            var parentCatalogs = myCatalogs.Where(c => c.MyCatalogId == null).ToList();

            var result = parentCatalogs.Select(c => GetCatalogTree(c, myCatalogs)).ToList();

            var serializedCatalogs = JsonConvert.SerializeObject(result, Formatting.Indented);

            // Визначення шляху до файлу в проекті
            var path = Path.Combine(Environment.CurrentDirectory, "output.txt");

            // Запис серіалізованих даних у файл
            File.WriteAllText(path, serializedCatalogs);
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
