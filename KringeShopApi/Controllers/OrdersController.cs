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
using KringeShopApi.HomeModel;

namespace KringeShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly KrinageShopDbContext _context;

        public OrdersController(KrinageShopDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            var orders = await _context.Orders.ToListAsync();

            //сортировка по дате заказа
            var result = orders.OrderByDescending(o => o.CreateDate);
            return Ok(result);
        }


        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            
            return Ok(order);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
           
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Create/{username}")]
        public async Task<ActionResult> PostOrder(string username, OrderDTO sent_order)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u=>u.Username==username);
            if (user == null) return NotFound();

            OrderStatus status = await _context.OrderStatuses.FirstOrDefaultAsync(o => o.Id == sent_order.StatusId);
            if (status == null) return NotFound();

            //добавление заказа
            Order order = new Order()
            {
                UserId=user.Id,
                Adress=sent_order.Adress,
                FullCost=sent_order.FullCost,
                RecieveDate=sent_order.RecieveDate,
                StatusId=sent_order.StatusId,
                CreateDate=sent_order.CreateDate,
                IsCmpleted=false,
                IsSelfPickUp=sent_order.IsSelfPickUp,
                //User=user,
                //Status=status
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            //добавление товаров в заказ
            var userBasket=await _context.BasketItems.Where(b=>b.UserId==user.Id).ToListAsync();

            foreach(var item in userBasket)
            {
                _context.OrderItems.Add(new OrderItem()
                {
                    ProductId=item.ProductId,
                    Count=item.Count,
                    Cost=item.Cost,
                    OrdeId=order.Id
                });
                _context.BasketItems.Remove(item);
            }
            await _context.SaveChangesAsync();

            

            if (await _context.Orders.ContainsAsync(order)) return Ok();
            else return BadRequest("Что-то пошло не так");
        }

        // DELETE: api/Orders/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            if (!await _context.Orders.ContainsAsync(order)) return Ok();
            else return BadRequest("Что-то пошло не так");
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
