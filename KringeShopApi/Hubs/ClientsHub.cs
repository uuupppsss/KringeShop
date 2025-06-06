﻿using Microsoft.AspNetCore.SignalR;
using KringeShopLib.Model;
using KringeShopApi.Model;
using Microsoft.AspNetCore.Mvc;
using KringeShopApi.HomeModel;

namespace KringeShopApi.Hubs
{
    public class ClientsHub : Hub
    {
        private KrinageShopDbContext _context;
        public ClientsHub(KrinageShopDbContext context)
        {
            _context = context;
        }

        //private async void ConnectClient(string username)
        //{
        //    //string name = Context.User.Identity.Name;
        //    if (!string.IsNullOrEmpty(username))
        //    {
        //        await Groups.AddToGroupAsync(Context.ConnectionId, username);
        //    }
        //    await Groups.AddToGroupAsync(Context.ConnectionId, "user");

        //}

        public async Task NewClientSignedUp()
        {
            await Clients.All.SendAsync("NewClientSignedUp");
        }

        public async Task OrderCreated(int order_id)
        {
            await Clients.All.SendAsync("OrderCreated", order_id);
        }

    }
}
