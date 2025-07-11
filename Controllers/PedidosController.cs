
using CrudMVCApp.Data;
using CrudMVCApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudMVCApp.Controllers
{
    public class PedidosController : Controller
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index(string buscarUsuario)
        {
            
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario")))
            {
                return RedirectToAction("Login", "Login");
            }

            
            var pedidosQuery = _context.Pedido
                .Include(p => p.Cliente)
                .Include(p => p.Detalles)
                .AsQueryable();

            
            if (!string.IsNullOrEmpty(buscarUsuario))
            {
                pedidosQuery = pedidosQuery.Where(p => p.Usuario.Contains(buscarUsuario));
            }

            var pedidos = await pedidosQuery.ToListAsync();
            return View(pedidos);
        }

        
        public async Task<IActionResult> Detalle(int id)
        {
            
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario")))
            {
                return RedirectToAction("Login", "Login");
            }

            var pedido = await _context.Pedido
                .Include(p => p.Cliente)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }
    }
}
