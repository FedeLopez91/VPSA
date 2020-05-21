using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VPSA.Data;
using VPSA.Models;

namespace VPSA.Controllers
{
    public class TipoDenunciasController : Controller
    {
        private readonly VPSAContext _context;

        public TipoDenunciasController(VPSAContext context)
        {
            _context = context;
        }

        // GET: TipoDenuncias
        public async Task<IActionResult> Index()
        {
            return View(await _context.TiposDenuncia.ToListAsync());
        }

        // GET: TipoDenuncias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDenuncia = await _context.TiposDenuncia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoDenuncia == null)
            {
                return NotFound();
            }

            return View(tipoDenuncia);
        }

        // GET: TipoDenuncias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoDenuncias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] TipoDenuncia tipoDenuncia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoDenuncia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoDenuncia);
        }

        // GET: TipoDenuncias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDenuncia = await _context.TiposDenuncia.FindAsync(id);
            if (tipoDenuncia == null)
            {
                return NotFound();
            }
            return View(tipoDenuncia);
        }

        // POST: TipoDenuncias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] TipoDenuncia tipoDenuncia)
        {
            if (id != tipoDenuncia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoDenuncia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoDenunciaExists(tipoDenuncia.Id))
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
            return View(tipoDenuncia);
        }

        // GET: TipoDenuncias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDenuncia = await _context.TiposDenuncia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoDenuncia == null)
            {
                return NotFound();
            }

            return View(tipoDenuncia);
        }

        // POST: TipoDenuncias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoDenuncia = await _context.TiposDenuncia.FindAsync(id);
            _context.TiposDenuncia.Remove(tipoDenuncia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoDenunciaExists(int id)
        {
            return _context.TiposDenuncia.Any(e => e.Id == id);
        }
    }
}
