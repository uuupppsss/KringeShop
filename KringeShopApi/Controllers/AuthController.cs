using KringeShopApi.HomeModel;
using KringeShopApi.Model;
using KringeShopLib.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KringeShopApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly KrinageShopDbContext _context;

        public AuthController(KrinageShopDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(UserDTO sent_user)
        {
            User found_user = await _context.Users.FirstOrDefaultAsync(u => u.Username == sent_user.Username);
            if (found_user != null) return BadRequest("Такой логин уже существует");

            User user = new User()
            {
                Username = sent_user.Username,
                Password = sent_user.Password,
                Email = sent_user.Email,
                ContactPhone = sent_user.ContactPhone,
                RoleId = sent_user.RoleId,
                Role = _context.UserRoles.FirstOrDefault(r => r.Id == sent_user.RoleId),
                BasketItems = new List<BasketItem>(),
                SavedProducts = new List<SavedProduct>(),
                Orders = new List<Order>()
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            if (await _context.Users.ContainsAsync(user)) return Ok();

            else return BadRequest("Что-то пошло не так");
        }

        // GET: api/Auth/admin/admin
        [HttpGet("{username}/{password}")]
        public async Task<ActionResult<ResponseTokenAndStuff>> SignIn(string username, string password)
        {
            var found_user = await _context.Users.Include(u=>u.Role).FirstOrDefaultAsync(u => u.Username == username);
            if (found_user == null) return Unauthorized("Пользователь с таким именем не найден");
            if (found_user.Password != password) return Unauthorized("Пароль не верный");

            var claims = new List<Claim>()
            {
                new Claim(ClaimValueTypes.Integer32, found_user.Id.ToString()),
                new Claim (ClaimTypes.Role, found_user.Role.Title),
            };

            var jwt = new JwtSecurityToken(
        issuer: AuthOptions.ISSUER,
        audience: AuthOptions.AUDIENCE,
        //кладём полезную нагрузку
        claims: claims,
        //устанавливаем время жизни токена 30
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new ResponseTokenAndStuff
            {
                Token = token,
                UserId= found_user.Id,
                Role = found_user.Role.Title,
                Email=found_user.Email,
                Phone=found_user.ContactPhone,
                Username=found_user.Username
            });
        }

        [HttpGet("IfUniqueEmail/{email}")]
        public async Task<ActionResult> IfUniqueEmail(string email)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (result == null) return Ok();
            else return BadRequest();

        }

        [HttpGet("IfUniqueUsername/{username}")]
        public async Task<ActionResult> IfUniqueUsername(string username)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (result == null) return Ok();
            else return BadRequest();
        }

        [HttpGet("IfUniquePhone/{phone}")]
        public async Task<ActionResult> IfUniquePhone(string phone)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.ContactPhone == phone);
            if (result == null) return Ok();
            else return BadRequest();
        }
    }
}
