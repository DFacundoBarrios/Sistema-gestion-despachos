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
    public class PersonasController : Controller
    {
        private readonly AppDbContext _context;

        public PersonasController(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Persona.ToListAsync());
        }

        
        public async Task<IActionResult> Details(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona
               
                .FirstOrDefaultAsync(m => m.Id == id);
              
                
            
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,Dni,Cuit,Direccion,Telefono,Email")] Persona persona)
        {
            
            var personaExistenteDni = await _context.Persona.FirstOrDefaultAsync(p => p.Dni == persona.Dni);
            if (personaExistenteDni != null)
            {
                ModelState.AddModelError("Dni", "Este DNI ya está registrado");
                TempData["Error"] = "El DNI ya está registrado en el sistema";
                return View(persona);
            }

            
            if (!string.IsNullOrEmpty(persona.Cuit))
            {
                var personaExistenteCuit = await _context.Persona.FirstOrDefaultAsync(p => p.Cuit == persona.Cuit);
                if (personaExistenteCuit != null)
                {
                    ModelState.AddModelError("Cuit", "Este CUIT ya está registrado");
                    TempData["Error"] = "El CUIT ya está registrado en el sistema";
                    return View(persona);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(persona);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cliente registrado correctamente";
                return RedirectToAction(nameof(Index));
            }
            return View(persona);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Dni,Cuit,Futbol,Basquet,Otros,Genero")] Persona persona)
        {
            if (id != persona.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(persona);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(persona.Id))
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
            return View(persona);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona
                .FirstOrDefaultAsync(m => m.Id == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var persona = await _context.Persona.FindAsync(id);
            if (persona != null)
            {
                _context.Persona.Remove(persona);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        private bool PersonaExists(int id)
        {
            return _context.Persona.Any(e => e.Id == id);
        }
    }
}
