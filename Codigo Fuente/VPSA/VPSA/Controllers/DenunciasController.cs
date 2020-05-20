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
    public class DenunciasController : Controller
    {
        private readonly VPSAContext _context;

        public DenunciasController(VPSAContext context)
        {
            _context = context;
        }

        // GET: Denuncias
        public async Task<IActionResult> Index()
        {
            var vPSAContext = _context.Denuncias.Include(d => d.EstadoDenuncia).Include(d => d.TipoDenuncia).OrderByDescending(o => o.Fecha);
            return View(await vPSAContext.ToListAsync());
        }

        // GET: Denuncias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var denuncia = await _context.Denuncias
                .Include(d => d.EstadoDenuncia)
                .Include(d => d.TipoDenuncia)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (denuncia == null)
            {
                return NotFound();
            }

            return View(denuncia);
        }

        // GET: Denuncias/Create
        public IActionResult Create()
        {
            //ViewData["EstadoDenunciaId"] = new SelectList(_context.EstadosDenuncia, "Id", "Id");
            ViewData["TipoDenunciaId"] = new SelectList(_context.TiposDenuncia, "Id", "Nombre");
            return View();
        }

        // POST: Denuncias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,Calle,Numero,EntreCalles1,EntreCalles2,Descripcion,TipoDenunciaId,EstadoDenunciaId,Legajo,IpDenunciante")] Denuncia denuncia)
        {
            denuncia.Fecha = DateTime.Now;
            _context.Add(denuncia);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Denuncia generada con Éxito" });
        }

        public IActionResult ThankYou()
        {
            return View();
        }

        // GET: Denuncias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var denuncia = await _context.Denuncias.FindAsync(id);
            if (denuncia == null)
            {
                return NotFound();
            }
            ViewData["EstadoDenunciaId"] = new SelectList(_context.EstadosDenuncia, "Id", "Id", denuncia.EstadoDenunciaId);
            ViewData["TipoDenunciaId"] = new SelectList(_context.TiposDenuncia, "Id", "Id", denuncia.TipoDenunciaId);
            return View(denuncia);
        }

        // POST: Denuncias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Calle,Numero,EntreCalles1,EntreCalles2,Descripcion,TipoDenunciaId,EstadoDenunciaId,Legajo,IpDenunciante")] Denuncia denuncia)
        {
            if (id != denuncia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(denuncia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DenunciaExists(denuncia.Id))
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
            ViewData["EstadoDenunciaId"] = new SelectList(_context.EstadosDenuncia, "Id", "Id", denuncia.EstadoDenunciaId);
            ViewData["TipoDenunciaId"] = new SelectList(_context.TiposDenuncia, "Id", "Id", denuncia.TipoDenunciaId);
            return View(denuncia);
        }

        // GET: Denuncias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var denuncia = await _context.Denuncias
                .Include(d => d.EstadoDenuncia)
                .Include(d => d.TipoDenuncia)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (denuncia == null)
            {
                return NotFound();
            }

            return View(denuncia);
        }

        // POST: Denuncias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var denuncia = await _context.Denuncias.FindAsync(id);
            _context.Denuncias.Remove(denuncia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DenunciaExists(int id)
        {
            return _context.Denuncias.Any(e => e.Id == id);
        }
    }
}
