using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KringeShopApi.Model;
using KringeShopLib.Model;
using KringeShopApi.HomeModel;
using Microsoft.AspNetCore.Authorization;

namespace KringeShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        private readonly KrinageShopDbContext _context;

        public OrderStatusController(KrinageShopDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderStatus
        [Authorize (Roles="admin")]
        [HttpGet]
        public async Task<ActionResult<List<OrderStatusDTO>>> GetOrderStatuses()
        {
            List<OrderStatusDTO> result = new();
            foreach (var s in _context.OrderStatuses)
            {
                result.Add(new OrderStatusDTO()
                {
                    Id= s.Id,
                    Title= s.Title
                });
            }
            return Ok(result);
        }

        // POST: api/OrderStatus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<OrderStatus>> PostOrderStatus(OrderStatus orderStatus)
        //{
        //    _context.OrderStatuses.Add(orderStatus);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetOrderStatus", new { id = orderStatus.Id }, orderStatus);
        //}

        // DELETE: api/OrderStatus/5

        
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOrderStatus(int id)
        //{
        //    var orderStatus = await _context.OrderStatuses.FindAsync(id);
        //    if (orderStatus == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.OrderStatuses.Remove(orderStatus);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

    }
}
