﻿using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Headers;

namespace KringeShopWebClient.Services
{
    public class Client
    {
        private static HttpClient _httpClient;
        public static HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                    _httpClient = new HttpClient
                    {
                        BaseAddress = new Uri("http://localhost:5216/api/")
                    };
                return _httpClient;
            }
        }

        //private static HubConnection _userHubConnection;
        //public static HubConnection UserHubConnection
        //{
        //    get
        //    {
        //        if(_userHubConnection == null)
        //            _userHubConnection = new HubConnectionBuilder()
        //        .WithUrl("http://localhost:5216/")
        //        .Build();
        //        return _userHubConnection;
        //    }
        //}

        //private static HubConnection _adminHubConnection;
        //public static HubConnection AdminHubConnection
        //{
        //    get
        //    {
        //        if (_adminHubConnection == null)
        //            _adminHubConnection = new HubConnectionBuilder()
        //        .WithUrl("http://localhost:5216/")
        //        .Build();
        //        return _adminHubConnection;
        //    }
        //}
    }
}
