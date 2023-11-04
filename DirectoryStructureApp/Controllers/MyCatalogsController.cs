using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DirectoryStructureApp.Data;
using DirectoryStructureApp.Interfaces;

namespace DirectoryStructureApp.Controllers
{
    public class MyCatalogsController : Controller
    {
        private readonly CatalogDbContext _context;
        private readonly IJsonFileService _jsonFileService;
        private readonly IMyCatalogRepository _myCatalogRepository;

        public MyCatalogsController(CatalogDbContext context, IJsonFileService jsonFileService, IMyCatalogRepository myCatalogRepository)
        {
            _context = context;
            _jsonFileService = jsonFileService;
            _myCatalogRepository = myCatalogRepository;
        }

        [HttpPost]
        [Route("MyCatalogs/SaveMyCatalogsToJsonFileAsync")]
        public async Task<IActionResult> SaveFile()
        {
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                return await _jsonFileService.SaveMyCatalogsToJsonFileAsync(body);
            }
        }


        [HttpPost]
        [Route("MyCatalogs/ImportDataFromJsonFile")]
        public IActionResult ImportDataFromJsonFile(IFormFile file)
        {
            try
            {
                _jsonFileService.ImportDataFromJsonFile(file);
                return RedirectToAction("Index");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [HttpPost]
        [Route("MyCatalogs/DeleteAllCatalogs")]
        public async Task<IActionResult> DeleteAllCatalogs()
        {
            await _myCatalogRepository.DeleteAllMyCatalogsAsync();
            return RedirectToAction("Index"); // Направлення на метод "Index" або іншу відповідну дію
        }

        [HttpPost]
        [Route("MyCatalogs/ImportDataFromJson")]
        public IActionResult ImportDataFromJson(IFormFile file)
        {
            return null;
        }


       
        public IActionResult Index()
        {
            var topLevelCatalogs = _context.MyCatalogs.Where(c => c.MyCatalogId == null).Include(c => c.Children).ToList();
            return View(topLevelCatalogs);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalog = _context.MyCatalogs.Include(c => c.Children).FirstOrDefault(c => c.Id == id);
            if (catalog == null)
            {
                return NotFound();
            }

            return View(catalog);
        }
    }

}


