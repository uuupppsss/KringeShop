using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KringeShopApi.Model;
using KringeShopLib.Model;
using Microsoft.AspNetCore.SignalR;
using System.Security.Cryptography.Xml;
using System.Data;

namespace KringeShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly KrinageShopDbContext _context;

        public UsersController(KrinageShopDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> SignUp(UserDTO sent_user)
        {
            User user = new User()
            {
                Username = sent_user.Username,
                Password = sent_user.Password,
                Email = sent_user.Email,
                ContactPhone = sent_user.ContactPhone,
                RoleId = sent_user.RoleId,
                Role= await _context.UserRoles.FirstOrDefaultAsync(r => r.Id == sent_user.RoleId),
                BasketItems = new List<BasketItem>(),
                SavedProducts = new List<SavedProduct>(),
                Orders = new List<Order>()
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            if (await _context.Users.ContainsAsync(user)) return Ok();

            else return BadRequest("Что-то пошло не так");
        }

        [HttpGet("SignIn/{username}/{password}")]
        public async Task<ActionResult<User>> SignIn(string password, string username)
        {
            var found_user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (found_user == null) return NotFound("Пользователь с тким именем не найден");
            if (found_user.Password != password) return BadRequest("Пароль не верный");
            else return Ok(found_user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            if (!await _context.Users.ContainsAsync(user)) return Ok();
            else return BadRequest("Что-то пошло не так");
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
