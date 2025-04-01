using KringeShopApi.Model;
using KringeShopLib.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KringeShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly KrinageShopDbContext _context;
        public AuthController(KrinageShopDbContext context)
        {
            _context = context;
        }

        // POST: api/Auth/SignUp
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost ("SignUp")]
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
                Role = await _context.UserRoles.FirstOrDefaultAsync(r => r.Id == sent_user.RoleId),
                BasketItems = new List<BasketItem>(),
                SavedProducts = new List<SavedProduct>(),
                Orders = new List<Order>()
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            if (await _context.Users.ContainsAsync(user)) return Ok();

            else return BadRequest("Что-то пошло не так");
        }

        // GET: api/Auth/SignIn/admin/admin
        [HttpGet("SignIn/{username}/{password}")]
        public async Task<ActionResult<ResponseTokenAndStuff>> SignIn(string username, string password)
        {
            var found_user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (found_user == null) return NotFound("Пользователь с тким именем не найден");
            if (found_user.Password != password) return Unauthorized("Пароль не верный");

            var claims = new List<Claim>()
            {
                new Claim(ClaimValueTypes.Integer32, found_user.Id.ToString()),
                new Claim (ClaimTypes.Role, found_user.Role.Title),
                //new Claim(ClaimTypes.Email, found_user.Email),
                //new Claim(ClaimTypes.MobilePhone, found_user.ContactPhone)
            };

            var jwt = new JwtSecurityToken(
        issuer: AuthOptions.ISSUER,
        audience: AuthOptions.AUDIENCE,
        //кладём полезную нагрузку
        claims: claims,
        //устанавливаем время жизни токена 2 минуты
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new ResponseTokenAndStuff
            {
                Token = token,
                Id= found_user.Id,
                Role = found_user.Role.Title,
                Email=found_user.Email,
                Phone=found_user.ContactPhone
            });
        }
    }
}
