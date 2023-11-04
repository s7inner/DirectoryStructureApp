using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DirectoryStructureApp.Data;
using DirectoryStructureApp.Models;
using DirectoryStructureApp.Interfaces;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

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
        [Route("MyCatalogs/SaveMyCatalogsToJsonFile")]
        public async Task<IActionResult> SaveFile()
        {
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                return await _jsonFileService.SaveMyCatalogsToJsonFile(body);
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
        public IActionResult DeleteAllCatalogs()
        {
            _myCatalogRepository.DeleteAllMyCatalogs();
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

        #region

        // GET: MyCatalogs
        public async Task<IActionResult> Index1()
        {
            return _context.MyCatalogs != null ?
                        View(await _context.MyCatalogs.ToListAsync()) :
                        Problem("Entity set 'CatalogDbContext.MyCatalogs'  is null.");
        }

        // GET: MyCatalogs/Details/5
        public async Task<IActionResult> Details1(int? id)
        {
            if (id == null || _context.MyCatalogs == null)
            {
                return NotFound();
            }

            var myCatalog = await _context.MyCatalogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myCatalog == null)
            {
                return NotFound();
            }

            return View(myCatalog);
        }

        // GET: MyCatalogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MyCatalogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,MyCatalogId")] MyCatalog myCatalog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(myCatalog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(myCatalog);
        }

        // GET: MyCatalogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MyCatalogs == null)
            {
                return NotFound();
            }

            var myCatalog = await _context.MyCatalogs.FindAsync(id);
            if (myCatalog == null)
            {
                return NotFound();
            }
            return View(myCatalog);
        }

        // POST: MyCatalogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,MyCatalogId")] MyCatalog myCatalog)
        {
            if (id != myCatalog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(myCatalog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MyCatalogExists(myCatalog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(myCatalog);
        }

        // GET: MyCatalogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MyCatalogs == null)
            {
                return NotFound();
            }

            var myCatalog = await _context.MyCatalogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myCatalog == null)
            {
                return NotFound();
            }

            return View(myCatalog);
        }

        // POST: MyCatalogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MyCatalogs == null)
            {
                return Problem("Entity set 'CatalogDbContext.MyCatalogs'  is null.");
            }
            var myCatalog = await _context.MyCatalogs.FindAsync(id);
            if (myCatalog != null)
            {
                _context.MyCatalogs.Remove(myCatalog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MyCatalogExists(int id)
        {
            return (_context.MyCatalogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        #endregion
    }

}


