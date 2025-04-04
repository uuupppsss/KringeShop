using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KringeShopApi.Model;
using KringeShopLib.Model;

namespace KringeShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemsController : ControllerBase
    {
        private readonly KrinageShopDbContext _context;

        public BasketItemsController(KrinageShopDbContext context)
        {
            _context = context;
        }

        // GET: api/BasketItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BasketItem>>> GetBasketItems()
        {
            return await _context.BasketItems.ToListAsync();
        }

        // GET: api/BasketItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BasketItem>> GetBasketItem(int id)
        {
            var basketItem = await _context.BasketItems.FindAsync(id);

            if (basketItem == null)
            {
                return NotFound();
            }

            return basketItem;
        }

        // PUT: api/BasketItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBasketItem(int id, int count)
        {
            BasketItem basketItem = await _context.BasketItems.FirstOrDefaultAsync(b => b.Id == id);
            if (basketItem == null) return BadRequest();

            basketItem.Count = count;
            _context.Entry(basketItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BasketItemExists(id))
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

        // POST: api/BasketItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostBasketItem(BasketItemDTO sent_basketItem)
        {
            if (sent_basketItem == null) return BadRequest(); 
            Product product= await _context.Products.FirstOrDefaultAsync(p=>p.Id== sent_basketItem.ProductId);
            if (product == null) return NotFound();
            User user= await _context.Users.FirstOrDefaultAsync(u=>u.Id==sent_basketItem.UserId);
            if (user == null) return NotFound();
            BasketItem basketItem = new BasketItem()
            {
                UserId = sent_basketItem.UserId,
                ProductId = sent_basketItem.ProductId,
                Cost = sent_basketItem.Cost,
                Count = sent_basketItem.Count,
                Product=product,
                User=user
            };
            _context.BasketItems.Add(basketItem);
            await _context.SaveChangesAsync();

            if (await _context.BasketItems.ContainsAsync(basketItem)) return Ok();
            else return BadRequest("Ошибка сохранения. Данны утеряны");

            
        }

        // DELETE: api/BasketItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasketItem(int id)
        {
            var basketItem = await _context.BasketItems.FindAsync(id);
            if (basketItem == null)
            {
                return NotFound();
            }

            _context.BasketItems.Remove(basketItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BasketItemExists(int id)
        {
            return _context.BasketItems.Any(e => e.Id == id);
        }
    }
}
