using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrudMVCApp.Data;
using CrudMVCApp.Models;

namespace CrudMVCApp.Controllers
{
    public class DireccionsController : Controller
    {
        private readonly AppDbContext _context;

        public DireccionsController(AppDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Create()
        {
            
            ViewData["PersonaId"] = new SelectList(_context.Persona, "Id", "Nombre");  
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Calle,Ciudad,CodigoPostal,PersonaId")] Direccion direccion)
        {
            if (ModelState.IsValid)
            {
             
                _context.Add(direccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); 
            }

       
            ViewData["PersonaId"] = new SelectList(_context.Persona, "Id", "Nombre", direccion.PersonaId);
            return View(direccion);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound(); 
            }

           
            var direccion = await _context.Direccion
                .Include(d => d.Persona)  
                .FirstOrDefaultAsync(m => m.Id == id);  

            if (direccion == null)
            {
                return NotFound();  
            }

            return View(direccion);  
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var direccion = await _context.Direccion.FindAsync(id);
            if (direccion != null)
            {
                _context.Direccion.Remove(direccion);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index)); 
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();  
            }

           
            var direccion = await _context.Direccion
                .Include(d => d.Persona)  
                .FirstOrDefaultAsync(m => m.Id == id);  

            if (direccion == null)
            {
                return NotFound();
            }

            return View(direccion); 
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

          
            var direccion = await _context.Direccion
                .Include(d => d.Persona)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (direccion == null)
            {
                return NotFound();
            }

            ViewData["PersonaId"] = new SelectList(_context.Persona, "Id", "Nombre", direccion.PersonaId);  
            return View(direccion); 
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Calle,Numero,Localidad,Provincia,PersonaId")] Direccion direccion)
        //{
        //    if (id != direccion.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(direccion);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!DireccionExists(direccion.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //    }

        //    ViewData["PersonaId"] = new SelectList(_context.Persona, "Id", "Nombre", direccion.PersonaId);
        //    return View(direccion);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Calle,Ciudad,CodigoPostal,PersonaId")] Direccion direccion)
        {
            if (id != direccion.Id)
            {
                return NotFound(); 
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(direccion);  
                    await _context.SaveChangesAsync();  
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DireccionExists(direccion.Id))
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
            ViewData["PersonaId"] = new SelectList(_context.Persona, "Id", "Nombre", direccion.PersonaId);  
            return View(direccion); 
        }

        private bool DireccionExists(int id)
        {
            return _context.Direccion.Any(e => e.Id == id); 
        }




        public async Task<IActionResult> Index()
        {
           
            var appDbContext = _context.Direccion.Include(d => d.Persona);
            return View(await appDbContext.ToListAsync());
        }
    }
}
