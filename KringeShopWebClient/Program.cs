using KringeShopWebClient.Auth;
using KringeShopWebClient.Components;
using KringeShopWebClient.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;

namespace KringeShopWebClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddAuthenticationCore();
            //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        options.Cookie.Name = "auth_token";
            //        options.LoginPath = "/SignIn";
            //        options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
            //        options.AccessDeniedPath = "/SignIn";
            //    });
            //builder.Services.AddAuthorization();
            builder.Services.AddCascadingAuthenticationState();
            //зависимости
            builder.Services.AddScoped<ProtectedSessionStorage>();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            //синглтоны
            builder.Services.AddSingleton<ConnectionService>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<UserService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();
            //app.UseAuthentication();
            //app.UseAuthorization();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
