using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KringeShopLib.Model;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("All/{loadedItemsCount}/{filterword}/{type_id}")]
        public async Task<ActionResult<List<ProductDTO>>> GetProducts(int loadedItemsCount,string filterword,int type_id)
        {
            filterword = filterword.ToLower();
            List<ProductDTO> result = new List<ProductDTO>();
            List<Product> products = new();
            List<Product> full_products_list = await _context.Products
                .Include(p => p.ProductImages).ToListAsync();
            if(filterword!="-")
                full_products_list=full_products_list.
                    Where(p => p.Name.ToLower().Contains(filterword)||
                (p.Description!=null&&p.Description.ToLower().Contains(filterword))).ToList();
            if (type_id != 0)
                full_products_list = full_products_list.
                    Where(p => p.TypeId == type_id).ToList();
            products = full_products_list
                 .Skip(loadedItemsCount).Take(18).ToList();
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
                    TimeBought= product.TimeBought,
                    Images=product.ProductImages.Select(x => Convert.ToBase64String(x.Image)).ToList(),
                    //CurrentImage= product.ProductImages.Select(x => Convert.ToBase64String(x.Image)).ToList().First()
                });
            }
            foreach (var r in result)
            {
                if(r.Images!=null&&r.Images.Count!=0)
                {
                    r.CurrentImage = r.Images[0];
                }
            }
            return Ok(result);
        }

        [HttpGet("Count/{filterword}/{type_id}")]
        public async Task<ActionResult<int>> GetProductsCount(string filterword, int type_id)
        {
            List<Product> result = await _context.Products.ToListAsync();
            if (filterword != "-") 
                result = result.Where(r => r.Name.Contains(filterword) 
                ||(r.Description!=null&&r.Description.Contains(filterword)) ).ToList();
            if(type_id!=0)
                result=result.Where(r=>r.TypeId==type_id).ToList();
            return Ok(result.Count);
        }

      
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound();
            ProductType type = await _context.ProductTypes.FirstOrDefaultAsync(t => t.Id == product.TypeId);
            if (type == null) return NotFound();    
            product.ProductImages=await _context.ProductImages.Where(i=>i.ProductId==product.Id).ToListAsync();

            ProductDTO result = new ProductDTO()
            {
                Id= product.Id,
                Name = product.Name,
                Description = product.Description,
                TypeId = product.TypeId,
                Type=type.Title,
                Price = product.Price,
                Count = product.Count,
                TimeBought = product.TimeBought,
                Images= product.ProductImages.Select(x => Convert.ToBase64String(x.Image)).ToList()
            };
            if(result.Images.Count!=0) result.CurrentImage = result.Images[0];
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
        public async Task<ActionResult<int>> PostProduct(ProductDTO sent_product)
        {
            Product product = new Product()
            {
                Name=sent_product.Name,
                Description=sent_product.Description,
                TypeId=sent_product.TypeId,
                Price=sent_product.Price,
                Count=sent_product.Count,
                TimeBought=0
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            if (await _context.Products.ContainsAsync(product)) return Ok(product.Id);
            else return BadRequest("Что то пошло не так");
        }

        [Authorize(Roles = "admin")]
        [HttpPost("Images/{product_id}")]
        public async Task<ActionResult> UploadProductImages(int product_id, List<byte[]> images)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == product_id);
            if (product is null) return NotFound();

            foreach(var image in images)
            {
                _context.ProductImages.Add(new ProductImage()
                {
                    ProductId = product_id,
                    Product = product,
                    Image = image
                });
            }
            await _context.SaveChangesAsync();
            return Ok();
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
