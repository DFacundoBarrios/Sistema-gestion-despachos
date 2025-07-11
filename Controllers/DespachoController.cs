using CrudMVCApp.Data;
using CrudMVCApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CrudMVCApp.Controllers
{
    public class DespachoController : Controller
    {
        private readonly AppDbContext _context;

        public DespachoController(AppDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            
            var usuario = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Login", "Login");
            }

            
            var pedidoActual = GetPedidoFromSession() ?? new Pedido
            {
                Usuario = usuario,
                Fecha = DateTime.Now,
                Detalles = new List<DetallePedido>()
            };

            
            SavePedidoToSession(pedidoActual);

            
            ViewBag.Clientes = new SelectList(_context.Persona, "Id", "Nombre");
            ViewBag.Productos = new SelectList(_context.Producto, "Id", "Nombre");

            return View(pedidoActual);
        }

        
        [HttpPost]
        public IActionResult SeleccionarCliente(int clienteId)
        {
            if (clienteId <= 0)
            {
                TempData["Error"] = "Debe seleccionar un cliente válido";
                return RedirectToAction(nameof(Index));
            }

            var cliente = _context.Persona.Find(clienteId);
            if (cliente == null)
            {
                TempData["Error"] = "Cliente no encontrado";
                return RedirectToAction(nameof(Index));
            }

            var pedido = GetPedidoFromSession();
            if (pedido == null)
            {
                return RedirectToAction(nameof(Index));
            }

            pedido.ClienteId = clienteId;
            pedido.Cliente = cliente;
            SavePedidoToSession(pedido);

            TempData["Success"] = $"Cliente {cliente.Nombre} {cliente.Apellido} seleccionado correctamente";
            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost]
        public IActionResult AgregarProducto(int productoId, int cantidad)
        {
            if (cantidad <= 0)
            {
                TempData["Error"] = "La cantidad debe ser mayor a cero";
                return RedirectToAction(nameof(Index));
            }

            var producto = _context.Producto.Find(productoId);
            if (producto == null)
            {
                TempData["Error"] = "Producto no encontrado";
                return RedirectToAction(nameof(Index));
            }

            
            if (producto.Stock < cantidad)
            {
                TempData["Error"] = $"Stock insuficiente. Solo hay {producto.Stock} unidades disponibles de {producto.Nombre}";
                return RedirectToAction(nameof(Index));
            }

            var pedido = GetPedidoFromSession();
            if (pedido == null)
            {
                return RedirectToAction(nameof(Index));
            }

            
            var detalle = pedido.Detalles.FirstOrDefault(d => d.ProductoId == productoId);
            if (detalle != null)
            {
                
                if (producto.Stock < (detalle.Cantidad + cantidad))
                {
                    TempData["Error"] = $"Stock insuficiente. Solo hay {producto.Stock} unidades disponibles de {producto.Nombre} y ya tiene {detalle.Cantidad} en su pedido";
                    return RedirectToAction(nameof(Index));
                }
                
                
                detalle.Cantidad += cantidad;
                detalle.PrecioUnitario = (decimal)producto.PrecioVta;
            }
            else
            {
                
                pedido.Detalles.Add(new DetallePedido
                {
                    ProductoId = productoId,
                    Producto = producto,
                    Cantidad = cantidad,
                    PrecioUnitario = (decimal)producto.PrecioVta
                });
            }

            SavePedidoToSession(pedido);
            TempData["Success"] = $"Se agregaron {cantidad} unidades de {producto.Nombre} al pedido";
            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost]
        public IActionResult ActualizarCantidad(int productoId, int cantidad)
        {
            if (cantidad <= 0)
            {
                TempData["Error"] = "La cantidad debe ser mayor a cero";
                return RedirectToAction(nameof(Index));
            }

            var producto = _context.Producto.Find(productoId);
            if (producto == null)
            {
                TempData["Error"] = "Producto no encontrado";
                return RedirectToAction(nameof(Index));
            }

            
            if (producto.Stock < cantidad)
            {
                TempData["Error"] = $"Stock insuficiente. Solo hay {producto.Stock} unidades disponibles de {producto.Nombre}";
                return RedirectToAction(nameof(Index));
            }

            var pedido = GetPedidoFromSession();
            if (pedido == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var detalle = pedido.Detalles.FirstOrDefault(d => d.ProductoId == productoId);
            if (detalle != null)
            {
                detalle.Cantidad = cantidad;
                SavePedidoToSession(pedido);
                TempData["Success"] = $"Se actualizó la cantidad de {producto.Nombre} a {cantidad} unidades";
            }

            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost]
        public async Task<IActionResult> ConfirmarPedido()
        {
            var pedido = GetPedidoFromSession();
            if (pedido == null || !pedido.Detalles.Any())
            {
                TempData["Error"] = "No hay productos en el pedido";
                return RedirectToAction(nameof(Index));
            }

            if (pedido.ClienteId == 0)
            {
                TempData["Error"] = "Debe seleccionar un cliente";
                return RedirectToAction(nameof(Index));
            }

            
            foreach (var detalle in pedido.Detalles)
            {
                var producto = await _context.Producto.FindAsync(detalle.ProductoId);
                if (producto == null)
                {
                    TempData["Error"] = "Uno de los productos ya no está disponible";
                    return RedirectToAction(nameof(Index));
                }

                if (producto.Stock < detalle.Cantidad)
                {
                    TempData["Error"] = $"Stock insuficiente. Solo hay {producto.Stock} unidades disponibles de {producto.Nombre}";
                    return RedirectToAction(nameof(Index));
                }
            }

            
            var nuevoPedido = new Pedido
            {
                Usuario = pedido.Usuario,
                Fecha = DateTime.Now,
                ClienteId = pedido.ClienteId,
                Confirmado = true
            };

            _context.Pedido.Add(nuevoPedido);
            await _context.SaveChangesAsync();

            
            foreach (var detalle in pedido.Detalles)
            {
                var nuevoDetalle = new DetallePedido
                {
                    PedidoId = nuevoPedido.id,
                    ProductoId = detalle.ProductoId,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario
                };

                _context.DetallePedido.Add(nuevoDetalle);
                
                
                var producto = await _context.Producto.FindAsync(detalle.ProductoId);
                if (producto != null)
                {
                    producto.Stock -= detalle.Cantidad;
                    _context.Update(producto);
                }
            }

            await _context.SaveChangesAsync();

            
            HttpContext.Session.Remove("PedidoActual");

            TempData["Success"] = "Pedido confirmado correctamente";
            return RedirectToAction("Index", "Pedidos");
        }

        
        private Pedido GetPedidoFromSession()
        {
            var pedidoJson = HttpContext.Session.GetString("PedidoActual");
            if (string.IsNullOrEmpty(pedidoJson))
            {
                return null;
            }

            var pedido = JsonSerializer.Deserialize<Pedido>(pedidoJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            
            if (pedido?.Detalles != null)
            {
                foreach (var detalle in pedido.Detalles)
                {
                    detalle.Producto = _context.Producto.Find(detalle.ProductoId);
                }
            }

            
            if (pedido?.ClienteId > 0)
            {
                pedido.Cliente = _context.Persona.Find(pedido.ClienteId);
            }

            return pedido;
        }

        private void SavePedidoToSession(Pedido pedido)
        {
            
            var pedidoParaSerializar = new Pedido
            {
                id = pedido.id,
                Usuario = pedido.Usuario,
                Fecha = pedido.Fecha,
                ClienteId = pedido.ClienteId,
                Confirmado = pedido.Confirmado,
                Detalles = pedido.Detalles.Select(d => new DetallePedido
                {
                    Id = d.Id,
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                }).ToList()
            };

            var pedidoJson = JsonSerializer.Serialize(pedidoParaSerializar);
            HttpContext.Session.SetString("PedidoActual", pedidoJson);
        }
    }
}