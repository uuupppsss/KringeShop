//я умная, правда :(
using Microsoft.AspNetCore.SignalR;
using KringeShopLib.Model;
using KringeShopApi.Model;

namespace KringeShopApi
{
    public class SellingHub:Hub
    {
        private KrinageShopDbContext _context;
        public SellingHub(KrinageShopDbContext context)
        {
            _context = context;
        }

        public async Task SignUserIn(User user)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, user.Role.Title);
        }

        public async Task OrderCreated(Order order)
        {
            await Clients.Group("admin").SendAsync("Ordercreated", order);
        }

        public async Task ProductOrdered(Product product)
        {
            await Clients.All.SendAsync("ProductOrdered", product.Id);
        }

        public async Task OrderInAssembly()
        {
            //заказ в сборке
        }

        public async Task OrderSent()
        {
            //заказ отправлен
        }

        public async Task OrderOnTheWay()
        {
            //заказ в пути
        }

        public async Task OrderIsReadyToRecieve()
        {
            //заказ готов к получению
        }

        public async Task OrderRecieved()
        {
            //заказ получен
        }
        


    }
}
