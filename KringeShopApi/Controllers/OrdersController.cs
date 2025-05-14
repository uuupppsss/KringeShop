using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KringeShopLib.Model;
using Microsoft.AspNetCore.Authorization;
using KringeShopApi.HomeModel;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static NuGet.Packaging.PackagingConstants;
using System.Collections.Generic;

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
        [HttpGet("ByStatus/{status_id}")]
        public async Task<ActionResult<List<OrderDTO>>> GetOrdersByStatus(int status_id)
        {
            List<Order> orders = new();
            if (status_id == 0)
            {
                orders = await _context.Orders.Include(o => o.Status).ToListAsync();
            }
            else
            {
                orders = await _context.Orders.Where(o => o.StatusId == status_id).Include(o => o.Status).ToListAsync();
            }
            //сортировка по дате заказа
            orders = orders.OrderByDescending(o => o.CreateDate).ToList();

            var result=new List<OrderDTO>();
            foreach (var order in orders)
            {
                result.Add(new OrderDTO()
                {
                    Id= order.Id,
                    UserId= order.UserId,
                    Adress=order.Adress,
                    FullCost=order.FullCost,
                    RecieveDate=order.RecieveDate,
                    StatusId=order.StatusId,
                    CreateDate=order.CreateDate,
                    //IsCmpleted=order.IsCmpleted,
                    IsSelfPickUp=order.IsSelfPickUp,
                    Status=order.Status.Title
                });
            }

            return Ok(result);
        }

        [Authorize(Roles = "user")]
        [HttpGet("ByUser/{status_id}")]
        public async Task<ActionResult<List<OrderDTO>>> GetUserOrders(int status_id)
        {
            var us = HttpContext.User.Claims.FirstOrDefault();
            int.TryParse(us.Value, out int user_id);

            List<Order> orders = new();

            if(status_id==1)
            orders = await _context.Orders.Where(o => o.UserId == user_id&&o.StatusId<4).
                    Include(o => o.Status).ToListAsync();
            else orders = await _context.Orders.Where(o => o.UserId == user_id && o.StatusId >=4).
                    Include(o => o.Status).ToListAsync();
            //сортировка по дате заказа
            orders = orders.OrderByDescending(o => o.CreateDate).ToList();

            var result = new List<OrderDTO>();
            foreach (var order in orders)
            {
                result.Add(new OrderDTO()
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    Adress = order.Adress,
                    FullCost = order.FullCost,
                    RecieveDate = order.RecieveDate,
                    StatusId = order.StatusId,
                    CreateDate = order.CreateDate,
                    //IsCmpleted = order.IsCmpleted,
                    IsSelfPickUp = order.IsSelfPickUp,
                    Status = order.Status.Title
                });
            }

            return Ok(result);
        }

        // GET: api/Orders/5
        [Authorize (Roles ="user,admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = await _context.OrderStatuses.FindAsync(order.StatusId);

            OrderDTO result = new OrderDTO()
            {
                Id=order.Id,
                UserId=order.UserId,
                Adress=order.Adress,
                FullCost=order.FullCost,
                RecieveDate=order.RecieveDate,
                StatusId=order.StatusId,
                Status=order.Status.Title,
                CreateDate=order.CreateDate,
                IsSelfPickUp=order.IsSelfPickUp
            };
            return Ok(result);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<ActionResult> PutOrder(OrderDTO sent_order)
        {
            Order found_order=await _context.Orders.FirstOrDefaultAsync(o=>o.Id==sent_order.Id);
            if (found_order == null) return BadRequest();

            found_order.StatusId=sent_order.StatusId;
            found_order.RecieveDate=sent_order.RecieveDate;

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize (Roles = "user")]
        [HttpPost("Create")]
        public async Task<ActionResult> PostOrder( OrderDTO sent_order)
        {
            int.TryParse(HttpContext.User.Claims.FirstOrDefault().Value, out int user_id);
            User user = await _context.Users.FirstOrDefaultAsync(u=>u.Id==user_id);
            if (user == null) return NotFound();

            OrderStatus status = await _context.OrderStatuses
                .FirstOrDefaultAsync(o => o.Id == sent_order.StatusId);
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
            var userBasket=await _context.BasketItems.Where(b=>b.UserId==user.Id).
                Include(i=>i.Product).ToListAsync();

            foreach(var item in userBasket)
            {
                item.Product.Count-=item.Count;
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
        //[Authorize(Roles = "admin")]
        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteOrder(int id)
        //{
        //    var order = await _context.Orders.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Orders.Remove(order);
        //    await _context.SaveChangesAsync();

        //    if (!await _context.Orders.ContainsAsync(order)) return Ok();
        //    else return BadRequest("Что-то пошло не так");
        //}


    }
}
