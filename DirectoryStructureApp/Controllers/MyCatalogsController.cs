using Microsoft.AspNetCore.Mvc;
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
        [Route("MyCatalogs/ImportDataFromJsonFileAsync")]
        public async Task<IActionResult> ImportDataFromJsonFileAsync(IFormFile file)
        {
            try
            {
                await _jsonFileService.ImportDataFromJsonFileAsync(file);
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
        [Route("MyCatalogs/DeleteAllCatalogsAsync")]
        public async Task<IActionResult> DeleteAllCatalogsAsync()
        {
            await _myCatalogRepository.DeleteAllMyCatalogsAsync();
            return RedirectToAction("Index"); // Направлення на метод "Index" або іншу відповідну дію
        }

        public async Task<IActionResult> Index()
        {
            var rootCatalog = await _myCatalogRepository.GetRootCatalogAsync();
            return View(rootCatalog);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalog = await _myCatalogRepository.GetCatalogByIdAsync(id.Value);
            if (catalog == null)
            {
                return NotFound();
            }

            return View(catalog);
        }

    }

}


