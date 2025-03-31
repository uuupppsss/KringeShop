using KringeShopApi.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace KringeShopApi
{
    public class AdminsHub:Hub
    {

        private KrinageShopDbContext _context;
        public AdminsHub(KrinageShopDbContext context)
        {
            _context = context;
        }
        public async Task OrderStatusChanged(int order_id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o=> o.Id==order_id);
            if (order == null) await Clients.Caller.SendAsync("Error", "Заказ не найден");
            var user = await _context.Users.FirstOrDefaultAsync(u=>u.Id==order.UserId);
            if (user == null) await Clients.Caller.SendAsync("Error", "пользователь не найден");
            Clients.Groups(user.Username).SendAsync("OrderStatusChanged", order.Status.Title);
        }
    }
}
