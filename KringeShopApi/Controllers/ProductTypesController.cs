﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KringeShopApi.Model;
using KringeShopLib.Model;
using KringeShopApi.HomeModel;

namespace KringeShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypesController : ControllerBase
    {
        private readonly KrinageShopDbContext _context;

        public ProductTypesController(KrinageShopDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductTypes
        [HttpGet]
        public async Task<ActionResult<List<ProductTypeDTO>>> GetProductTypes()
        {
            List<ProductTypeDTO> result = new();
            foreach (var productType in _context.ProductTypes)
            {
                result.Add(new ProductTypeDTO()
                {
                    Id = productType.Id,
                    Title = productType.Title
                });
            }
            return Ok(result);
        }


        

        // POST: api/ProductTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductType>> PostProductType(ProductType productType)
        {
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductType", new { id = productType.Id }, productType);
        }

        // DELETE: api/ProductTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }

            _context.ProductTypes.Remove(productType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

       
    }
}
