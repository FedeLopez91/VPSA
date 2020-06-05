using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VPSA.Data;
using VPSA.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace VPSA.Controllers
{
    public class DenunciasController : Controller
    {
        private readonly VPSAContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public DenunciasController(VPSAContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        // GET: Denuncias
        public ActionResult Index(string sortOrder)
        {
            //Get all demands
            var vPSAContext = _context.Denuncias.Include(d => d.EstadoDenuncia).Include(d => d.TipoDenuncia).OrderByDescending(o => o.Fecha);

            //Asks if you've been already here -> Change the sort order.
            ViewBag.FechaSortParam = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.AddrSortParam = sortOrder == "Addr" ? "Addr_desc" : "Addr";
            ViewBag.TypeDSortParam = sortOrder == "tipo_d" ? "tipo_d_desc" : "tipo_d";
            ViewBag.StateSortParam = sortOrder == "state" ? "state_desc" : "state_open";
            
            switch (sortOrder)
            {
                //Remove closed demands
                case "state_open":
                vPSAContext = _context.Denuncias.Include(d => d.EstadoDenuncia).Include(d => d.TipoDenuncia).Where(d => d.Calle != "d 124").OrderByDescending(o => o.Fecha);
                    break;

                //Address field
                case "Addr_desc":
                    vPSAContext = vPSAContext.OrderByDescending(o => o.Calle);
                    break;
                case "Addr":
                    vPSAContext = vPSAContext.OrderBy(o => o.Calle);
                    break;

                //State field
                case "state_desc":
                    vPSAContext = vPSAContext.OrderByDescending(o => o.EstadoDenuncia);
                    break;
                case "state":
                    vPSAContext = vPSAContext.OrderBy(o => o.EstadoDenuncia);
                    break;

                //Demand field
                case "tipo_d":
                    vPSAContext = vPSAContext.OrderBy(o => o.TipoDenuncia);
                    break;
                case "tipo_d_desc":
                    vPSAContext = vPSAContext.OrderByDescending(o => o.TipoDenuncia);
                    break;
                
                //Date field
                case "date":
                    vPSAContext = vPSAContext.OrderBy(o => o.Fecha);
                    break;
                case "date_desc":
                    vPSAContext = vPSAContext.OrderByDescending(o => o.Fecha);
                    break;
            }
            
            return View(vPSAContext.ToList());
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

            var PhotoUrl = _configuration.GetValue<string>("myKeys:PhotosUrl") + denuncia.NroDenuncia+".jpg";
            ViewData["Comentarios"] = await _context.Comentarios.Where(x => x.DenunciaId == denuncia.Id)
                .Include(d => d.EstadoDenuncia).Include(d=>d.Empleado).ToListAsync();
            ViewBag.Hasphoto = false;
            if (System.IO.File.Exists(PhotoUrl))
            {
                ViewBag.Hasphoto = true;
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
        public async Task<IActionResult> Create([Bind("Id,Fecha,Calle,Numero,EntreCalles1,EntreCalles2,Descripcion,TipoDenunciaId,EstadoDenunciaId,Legajo,IpDenunciante, Foto")] DenunciaViewModel denunciaViewModel)
        {
            if (ModelState.IsValid)
            {
                var denuncia = _mapper.Map<Denuncia>(denunciaViewModel);
                var nroDenuncia = 0;
                if (_context.Denuncias.Count() > 0)
                {
                    nroDenuncia = _context.Denuncias.Max(x => x.Id);
                }
                nroDenuncia = nroDenuncia == 0 ? 1 : nroDenuncia + 1;
                denuncia.NroDenuncia = $"D-{nroDenuncia.ToString().PadLeft(8, '0')}";
                denuncia.Fecha = DateTime.Now;
                _context.Add(denuncia);
                await _context.SaveChangesAsync();
                if (denunciaViewModel.Foto != null)
                    await UploadFile(denunciaViewModel.Foto, denuncia.NroDenuncia);

                return RedirectToAction("ThankYou", new { id = denuncia.Id });
            }
            return View(denunciaViewModel);
        }

        public async Task<IActionResult> ThankYou(int? id)
        {
            var denuncia = await _context.Denuncias.Where(x => x.Id == id).FirstOrDefaultAsync();
            return View(denuncia);
        }

        [HttpPost]
        public async Task<IActionResult> View(string NroDenuncia)
        {
            var denuncia = await _context.Denuncias.Where(x => x.NroDenuncia == NroDenuncia).Include(d => d.EstadoDenuncia)
                .Include(d => d.TipoDenuncia).FirstOrDefaultAsync();
            
            if (denuncia != null)
            {
                ViewData["Comentarios"] = await _context.Comentarios.Where(x => x.DenunciaId == denuncia.Id)
                   .Include(d => d.EstadoDenuncia).Include(d => d.Empleado).ToListAsync();
                var PhotoUrl = _configuration.GetValue<string>("myKeys:PhotosUrl") + denuncia.NroDenuncia + ".jpg";

                ViewBag.Hasphoto = false;
                if (System.IO.File.Exists(PhotoUrl))
                {
                    ViewBag.Hasphoto = true;
                }
            }


            return View("Details", denuncia);
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

        private async Task<IActionResult> UploadFile(IFormFile File, string PhotoName)
        {
            try
            {
                var PhotoUrl = _configuration.GetValue<string>("myKeys:PhotosUrl");

                if (!Directory.Exists(PhotoUrl))
                {
                    Directory.CreateDirectory(PhotoUrl);
                }

                if (File.Length > 0)
                {
                    string folderRoot = Path.Combine(PhotoUrl);
                    string filePath = PhotoName + ".jpg";
                    filePath = Path.Combine(folderRoot, filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await File.CopyToAsync(stream);
                    }
                }
                return Ok(new { success = true, message = "File Uploaded" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Error file failed to upload" });
            }
        }
    }
}
