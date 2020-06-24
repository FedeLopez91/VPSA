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
    public class ComentariosController : Controller
    {
        private readonly VPSAContext _context;

        public ComentariosController(VPSAContext context)
        {
            _context = context;
        }

        // GET: Comentarios
        public async Task<IActionResult> Index()
        {
            var vPSAContext = _context.Comentarios.Include(c => c.Denuncia).Include(c => c.User);
            return View(await vPSAContext.ToListAsync());
        }

        // GET: Comentarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comentario = await _context.Comentarios
                .Include(c => c.Denuncia)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comentario == null)
            {
                return NotFound();
            }

            return View(comentario);
        }

        // GET: Comentarios/Create
        public async Task<IActionResult> Create(int Id)
        {
            var denuncia = await _context.Denuncias.Where(x => x.Id == Id).Include(d => d.TipoDenuncia).FirstOrDefaultAsync();
            ViewData["Denuncia"] = denuncia;
            ViewData["EstadoId"] = new SelectList(_context.Set<EstadoDenuncia>(), "Id", "Nombre");
            ViewData["EmpleadoId"] = new SelectList(_context.Set<User>(), "Id", "NombreCompleto");
            return View();
        }

        // POST: Comentarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Descripcion,DenunciaId,UserId,EstadoDenunciaId")] Comentario comentario)
        {
            try
            {
                var denuncia = await _context.Denuncias.FindAsync(comentario.DenunciaId);
                denuncia.EstadoDenunciaId = comentario.EstadoDenunciaId;
                comentario.FechaCreacion = DateTime.Now;
                _context.Add(comentario);
                _context.Update(denuncia);
                await _context.SaveChangesAsync();

                ViewData["EstadoId"] = new SelectList(_context.Set<EstadoDenuncia>(), "Id", "Nombre");
                ViewData["EmpleadoId"] = new SelectList(_context.Set<User>(), "Id", "NombreCompleto");

                return Ok(new { success = true, message = "Trabajo cargado con Éxito", denunciaId = comentario.DenunciaId });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message, denunciaId = comentario.DenunciaId });
            }

        }

        // GET: Comentarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario == null)
            {
                return NotFound();
            }
            ViewData["DenunciaId"] = new SelectList(_context.Denuncias, "Id", "Id", comentario.DenunciaId);
            ViewData["EmpleadoId"] = new SelectList(_context.Users, "Id", "Id", comentario.UserId);
            return View(comentario);
        }

        // POST: Comentarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,DenunciaId,EmpleadoId,Estadoid")] Comentario comentario)
        {
            if (id != comentario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comentario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComentarioExists(comentario.Id))
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
            ViewData["DenunciaId"] = new SelectList(_context.Denuncias, "Id", "Id", comentario.DenunciaId);
            ViewData["EmpleadoId"] = new SelectList(_context.Users, "Id", "Id", comentario.UserId);
            return View(comentario);
        }

        // GET: Comentarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comentario = await _context.Comentarios
                .Include(c => c.Denuncia)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comentario == null)
            {
                return NotFound();
            }

            return View(comentario);
        }

        // POST: Comentarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);
            _context.Comentarios.Remove(comentario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> LoadData(int? denunciaId)
        {
            if (denunciaId == null)
            {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, data = "[]" });
            }

            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 10;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data  
                var comentarios = await _context.Comentarios.Where(x => x.DenunciaId == denunciaId).OrderByDescending(x => x.FechaCreacion).Select(x => new ComentarioViewModel
                {
                    FechaCreacion = x.FechaCreacion.ToString("dd/MM/yyyy HH:mm"),
                    Legajo = x.User.Legajo.ToString(),
                    Empleado = x.User.NombreCompleto,
                    Descripcion = x.Descripcion,
                    EstadoDenuncia = x.EstadoDenuncia.Nombre,
                }).ToListAsync();

                //Sorting  
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                //{
                //    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                //}
                ////Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    comentarios = comentarios.Where(m => m.Descripcion.ToLower().Contains(searchValue.ToLower())
                                || m.Empleado.ToLower().Contains(searchValue.ToLower())
                                || m.EstadoDenuncia.ToLower().Contains(searchValue.ToLower())).ToList();
                }

                //total number of rows count   
                recordsTotal = comentarios.Count();
                //Paging   
                var data = comentarios.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private bool ComentarioExists(int id)
        {
            return _context.Comentarios.Any(e => e.Id == id);
        }
    }
}
