using Blazored.SessionStorage;
using KringeShopWebClient.Model;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace KringeShopWebClient.Auth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        
        private readonly ISessionStorageService _sessionStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(ISessionStorageService sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSession = await _sessionStorage.GetItemAsync<UserSession>("UserSession");
                if (userSession is null)
                    return await Task.FromResult(new AuthenticationState(_anonymous));
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.Username),
                    new Claim(ClaimTypes.Role, userSession.Role)
                }, "CustomAuth"));
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }

        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            ClaimsPrincipal claimsPrincipal;
            if (userSession != null)
            {
                await _sessionStorage.SetItemAsync("UserSession", userSession);
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                   new Claim(ClaimTypes.Name, userSession.Username),
                   new Claim(ClaimTypes.Role, userSession.Role),
                }, "CustomAuth"));
            }
            else
            {
                await _sessionStorage.RemoveItemAsync("UserSession");
                claimsPrincipal = _anonymous;
            }

            var authState = new AuthenticationState(claimsPrincipal);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

    }
}
