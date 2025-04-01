using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KringeShopApi.Model;
using KringeShopLib.Model;
using Microsoft.AspNetCore.Authorization;

namespace KringeShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly KrinageShopDbContext _context;

        public ProductsController(KrinageShopDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            return Ok(await _context.Products.ToListAsync());
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }

            
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            if (await _context.Products.ContainsAsync(product)) return Ok();
            else return BadRequest("Что то пошло не так");
        }

        // DELETE: api/Products/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            if (!await _context.Products.ContainsAsync(product)) return Ok();
            else return BadRequest("Что то пошло не так");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
