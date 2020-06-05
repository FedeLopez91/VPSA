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
    public class EstadoDenunciasController : Controller
    {
        private readonly VPSAContext _context;

        public EstadoDenunciasController(VPSAContext context)
        {
            _context = context;
        }

        // GET: EstadoDenuncias
        public async Task<IActionResult> Index()
        {
            return View(await _context.EstadosDenuncia.ToListAsync());
        }

        // GET: EstadoDenuncias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoDenuncia = await _context.EstadosDenuncia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadoDenuncia == null)
            {
                return NotFound();
            }

            return View(estadoDenuncia);
        }

        // GET: EstadoDenuncias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadoDenuncias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] EstadoDenuncia estadoDenuncia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadoDenuncia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadoDenuncia);
        }

        // GET: EstadoDenuncias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoDenuncia = await _context.EstadosDenuncia.FindAsync(id);
            if (estadoDenuncia == null)
            {
                return NotFound();
            }
            return View(estadoDenuncia);
        }

        // POST: EstadoDenuncias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] EstadoDenuncia estadoDenuncia)
        {
            if (id != estadoDenuncia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadoDenuncia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoDenunciaExists(estadoDenuncia.Id))
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
            return View(estadoDenuncia);
        }

        // GET: EstadoDenuncias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoDenuncia = await _context.EstadosDenuncia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadoDenuncia == null)
            {
                return NotFound();
            }

            return View(estadoDenuncia);
        }

        // POST: EstadoDenuncias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estadoDenuncia = await _context.EstadosDenuncia.FindAsync(id);
            _context.EstadosDenuncia.Remove(estadoDenuncia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadoDenunciaExists(int id)
        {
            return _context.EstadosDenuncia.Any(e => e.Id == id);
        }
    }
}
