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
using System.Reflection.Metadata;
using KringeShopApi.HomeModel;

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
        public async Task<ActionResult<List<ProductDTO>>> GetProducts()
        {
            List<ProductDTO> result = new List<ProductDTO>();
            List<Product> products = new();
           products = await _context.Products.ToListAsync();
            foreach (var product in products)
            {
                result.Add(new ProductDTO()
                {
                    Id= product.Id,
                    Name= product.Name,
                    Description= product.Description,
                    TypeId= product.TypeId,
                    Price= product.Price,
                    Count= product.Count,
                    TimeBought= product.TimeBought
                });
            }
            return Ok(result);
        }

        // GET: api/Products/Filter/filterword
        [HttpGet("Filter/{filterword}")]
        public async Task<ActionResult<List<ProductDTO>>> GetFilteredProducts(string filterword)
        {
            List<ProductDTO> result = new List<ProductDTO>();
            List<Product> products = new();
            products = await _context.Products.Where(p=>p.Name.Contains(filterword)||p.Description.Contains(filterword)).ToListAsync();
            foreach (var product in products)
            {
                result.Add(new ProductDTO()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    TypeId = product.TypeId,
                    Price = product.Price,
                    Count = product.Count,
                    TimeBought = product.TimeBought
                });
            }
            return Ok(result);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound();
            ProductType type = await _context.ProductTypes.FirstOrDefaultAsync(t => t.Id == product.TypeId);
            if (type == null) return NotFound();    

            ProductDTO result = new ProductDTO()
            {
                Id= product.Id,
                Name = product.Name,
                Description = product.Description,
                TypeId = product.TypeId,
                Type=type.Title,
                Price = product.Price,
                Count = product.Count,
                TimeBought = product.TimeBought
            };
            return Ok(result);
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

        //[Authorize(Roles = "admin")]
        [HttpPost("Upload")]
        public async Task<ActionResult> UploadProductImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var filePath = Path.Combine("uploads", file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { FilePath = filePath });
        }
    }
}
