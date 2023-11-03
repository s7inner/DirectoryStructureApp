using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DirectoryStructureApp.Data;
using DirectoryStructureApp.Models;

namespace DirectoryStructureApp.Controllers
{
    public class MyCatalogsController : Controller
    {
        private readonly CatalogDbContext _context;

        public MyCatalogsController(CatalogDbContext context)
        {
            _context = context;
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


