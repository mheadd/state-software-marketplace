using Microsoft.AspNetCore.Mvc;
using state_software_marketplace.Data;
using state_software_marketplace.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace state_software_marketplace.Controllers
{
    public class SoftwareProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SoftwareProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SoftwareProducts
        public async Task<IActionResult> Index()
        {
            var products = await _context.SoftwareProducts.ToListAsync();
            return View(products);
        }

        // GET: SoftwareProducts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SoftwareProducts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Vendor,Category")] SoftwareProduct product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: SoftwareProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.SoftwareProducts.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: SoftwareProducts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Vendor,Category")] SoftwareProduct product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoftwareProductExists(product.Id))
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
            return View(product);
        }

        // GET: SoftwareProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.SoftwareProducts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: SoftwareProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.SoftwareProducts.FindAsync(id);
            if (product != null)
            {
                _context.SoftwareProducts.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SoftwareProductExists(int id)
        {
            return _context.SoftwareProducts.Any(e => e.Id == id);
        }
    }
}
