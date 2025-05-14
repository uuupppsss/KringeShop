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
    public class BasketItemsController : ControllerBase
    {
        private readonly KrinageShopDbContext _context;

        public BasketItemsController(KrinageShopDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "user")]
        // GET: api/BasketItems/GetUsersBasketItems
        [HttpGet("GetUsersBasketItems")]
        public async Task<ActionResult<List<BasketItemDTO>>> GetUsersBasketItems()
        {
            var us = HttpContext.User.Claims.FirstOrDefault();
            int.TryParse(us.Value, out int user_id);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id==user_id);
            if (user is null) return NotFound();
            List<BasketItem> basketItems= await _context.BasketItems.Where(b=>b.UserId==user_id).Include(b=>b.Product).ToListAsync();
            List<BasketItemDTO> result = new();
            if (basketItems.Count != 0)
            {
                foreach (var item in basketItems)
                {
                    result.Add(new BasketItemDTO()
                    {
                        Id = item.Id,
                        UserId = user_id,
                        ProductId = item.ProductId,
                        ProductName=item.Product.Name,
                        Count = item.Count,
                        Cost = item.Cost
                    });
                }
            }
            return Ok(result);
        }

        // GET: api/BasketItems/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<BasketItem>> GetBasketItem(int id)
        //{
        //    var basketItem = await _context.BasketItems.FindAsync(id);

        //    if (basketItem == null)
        //    {
        //        return NotFound();
        //    }

        //    return basketItem;
        //}

        [Authorize (Roles = "user")]
        // PUT: api/BasketItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutBasketItem(BasketItemDTO basketItem)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(product => product.Id == basketItem.ProductId);
            if (product == null) return NotFound();

            if (basketItem.Count > product.Count) return NoContent();

            BasketItem found_basketItem = await _context.BasketItems.FirstOrDefaultAsync(b => b.Id == basketItem.Id);
            if (basketItem == null) return NotFound();

            User user = await _context.Users.FirstOrDefaultAsync(user => user.Id == basketItem.UserId);
            if (user == null) return NotFound();

            found_basketItem.Count = basketItem.Count;
            found_basketItem.Cost = basketItem.Cost;

            //_context.Entry(basketItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            //    if (!BasketItemExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            }

            return Ok();
        }

        [Authorize(Roles = "user")]
        // POST: api/BasketItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostBasketItem( BasketItemDTO sent_basketItem)
        {
            var us=HttpContext.User.Claims.FirstOrDefault();
            int.TryParse(us.Value, out int user_id);
            if (sent_basketItem == null) return BadRequest(); 
            Product product= await _context.Products.FirstOrDefaultAsync(p=>p.Id== sent_basketItem.ProductId);
            if (product == null) return NotFound();
            User user= await _context.Users.FirstOrDefaultAsync(u=>u.Id==user_id);
            if (user == null) return NotFound();
            BasketItem basketItem = new BasketItem()
            {
                UserId = user.Id,
                ProductId = sent_basketItem.ProductId,
                Cost = sent_basketItem.Cost,
                Count = sent_basketItem.Count,
                //Product = product,
                //User = user
            };
            _context.BasketItems.Add(basketItem);
            await _context.SaveChangesAsync();

            if (await _context.BasketItems.ContainsAsync(basketItem)) return Ok();
            else return BadRequest("Ошибка сохранения. Данны утеряны");

            
        }

        [Authorize (Roles ="user")]
        // DELETE: api/BasketItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBasketItem(int id)
        {
            var basketItem = await _context.BasketItems.FindAsync(id);
            if (basketItem == null)
            {
                return NotFound();
            }

            _context.BasketItems.Remove(basketItem);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Authorize (Roles ="user")]
        [HttpGet("GetMaxCount/{id}")]
        public async Task<ActionResult<int>> GetBasketItemMaxCount(int id)
        {
            BasketItem basketItem=await _context.BasketItems.FirstOrDefaultAsync(e => e.Id == id);
            if (basketItem == null) return NotFound();
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketItem.ProductId);
            if (product == null) return NotFound();
            return Ok(product.Count);
        }
    }
}
