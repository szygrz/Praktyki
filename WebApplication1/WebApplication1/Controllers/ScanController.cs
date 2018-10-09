using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ScanController : Controller
    {
        private readonly WebApplication1Context _context;

        public ScanController(WebApplication1Context context)
        {
            _context = context;
        }

        // GET: Scan
        public async Task<IActionResult> Index()
        {
         

            return View(await _context.Scan.ToListAsync());
        }

        // GET: Scan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scan = await _context.Scan
                .SingleOrDefaultAsync(m => m.ID == id);
            if (scan == null)
            {
                return NotFound();
            }

            return View(scan);
        }

        // GET: Scan/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Scan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FilesNum,FilesInf,Size,TimeRunn,Recognition,Antyvirus")] Scan scan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scan);
        }

        // GET: Scan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scan = await _context.Scan.SingleOrDefaultAsync(m => m.ID == id);
            if (scan == null)
            {
                return NotFound();
            }
            return View(scan);
        }

        // POST: Scan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FilesNum,FilesInf,Size,TimeRunn,Recognition,Antyvirus")] Scan scan)
        {
            if (id != scan.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScanExists(scan.ID))
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
            return View(scan);
        }

        // GET: Scan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scan = await _context.Scan
                .SingleOrDefaultAsync(m => m.ID == id);
            if (scan == null)
            {
                return NotFound();
            }

            return View(scan);
        }

        // POST: Scan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scan = await _context.Scan.SingleOrDefaultAsync(m => m.ID == id);
            _context.Scan.Remove(scan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScanExists(int id)
        {
            return _context.Scan.Any(e => e.ID == id);
        }
    }
}
