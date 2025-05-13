using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KringeShopApi.HomeModel;
using KringeShopLib.Model;
using KringeShopApi.Model;

namespace KringeShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly KrinageShopDbContext _context;

        public OrderItemsController(KrinageShopDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderItems
        [HttpGet("{order_id}")]
        public async Task<ActionResult<List<OrderItemDTO>>> GetOrderItems(int order_id)
        {
            List<OrderItemDTO> result = new();
            foreach (var item in _context.OrderItems.Include(i=>i.Product).Where(i=>i.OrdeId==order_id))
            {
                result.Add(new OrderItemDTO()
                {
                    Id= item.Id,
                    ProductId= item.ProductId,
                    ProductName=item.Product.Name,
                    Count=item.Count,
                    Cost=item.Cost,
                    OrdeId=item.OrdeId
                });
            }
            return Ok(result);
        }

        // GET: api/OrderItems/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        //{
        //    var orderItem = await _context.OrderItems.FindAsync(id);

        //    if (orderItem == null)
        //    {
        //        return NotFound();
        //    }

        //    return orderItem;
        //}

        // PUT: api/OrderItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(int id, OrderItem orderItem)
        {
            if (id != orderItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(orderItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/OrderItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderItem", new { id = orderItem.Id }, orderItem);
        }

        // DELETE: api/OrderItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItems.Any(e => e.Id == id);
        }
    }
}
