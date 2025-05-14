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

    }
}
