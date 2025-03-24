//я умная, правда :(
using Microsoft.AspNetCore.SignalR;
using KringeShopLib.Model;

namespace KringeShopApi
{
    public class SellingHub:Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync(); 
        }

        public async Task OrderCreated(Order order)
        {
            //пользователь создал заказ
        }

        public async Task ProductOrdered(Product product)
        {
            //единица товара заказана
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
