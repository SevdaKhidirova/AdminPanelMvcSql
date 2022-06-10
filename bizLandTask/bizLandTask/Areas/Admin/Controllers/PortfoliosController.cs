using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bizLandTask.DAL;
using bizLandTask.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace bizLandTask.Areas.Admin
{
    [Area("Admin")]
    public class PortfoliosController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public PortfoliosController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Admin/Portfolios
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Portfolios.Include(p => p.Category);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/Portfolios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolio = await _context.Portfolios
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (portfolio == null)
            {
                return NotFound();
            }

            return View(portfolio);
        }

        // GET: Admin/Portfolios/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Admin/Portfolios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Portfolio portfolio)
        {
            if (!portfolio.Img.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Img", "File is not image");
                return View();
            }
            if (portfolio.Img.Length / 1024 > 400)
            {
                ModelState.AddModelError("Img", "File is too big");
                return View();
            }

            string path = _env.WebRootPath + @"\img\portfolio";
            string fileName = Guid.NewGuid().ToString() + portfolio.Img.FileName;
            string final = Path.Combine(path, fileName);

            using(FileStream stream = new FileStream(final, FileMode.Create))
            {
                await portfolio.Img.CopyToAsync(stream);
            }
            portfolio.Image = fileName;


            if (ModelState.IsValid)
            {
                _context.Add(portfolio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", portfolio.CategoryId);
            return View(portfolio);
        }

        // GET: Admin/Portfolios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolio = await _context.Portfolios.FindAsync(id);
            if (portfolio == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", portfolio.CategoryId);
            return View(portfolio);
        }

        // POST: Admin/Portfolios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Portfolio portfolio, int id)
        {
            if (id != portfolio.Id)
            {
                return NotFound();
            }
            if (portfolio.Img != null)
            {
                if (!portfolio.Img.ContentType.Contains("image"))
                {
                    ModelState.AddModelError("Img", "File is not image");
                    return View();
                }
                if (portfolio.Img.Length / 1024 > 400)
                {
                    ModelState.AddModelError("Img", "File is too big");
                    return View();
                }

                string path = _env.WebRootPath + @"\img\portfolio";
                string fileName = Guid.NewGuid().ToString() + portfolio.Img.FileName;
                string final = Path.Combine(path, fileName);


                if (System.IO.File.Exists(Path.Combine(path,portfolio.Image)))
                {
                    System.IO.File.Delete(Path.Combine(path, portfolio.Image));
                }

                using (FileStream stream = new FileStream(final, FileMode.Create))
                {
                    await portfolio.Img.CopyToAsync(stream);
                }
                portfolio.Image = fileName;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(portfolio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PortfolioExists(portfolio.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", portfolio.CategoryId);
            return View(portfolio);
        }

        // GET: Admin/Portfolios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolio = await _context.Portfolios
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (portfolio == null)
            {
                return NotFound();
            }

            return View(portfolio);
        }

        // POST: Admin/Portfolios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var portfolio = await _context.Portfolios.FindAsync(id);

            string path = _env.WebRootPath + @"\img\portfolio";
            string fileName = portfolio.Image;
            string final = Path.Combine(path, fileName);

            if (System.IO.File.Exists(final))
            {
                System.IO.File.Delete(final);
            }

            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PortfolioExists(int id)
        {
            return _context.Portfolios.Any(e => e.Id == id);
        }
    }
}
