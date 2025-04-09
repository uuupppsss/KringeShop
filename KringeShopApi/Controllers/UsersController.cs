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
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        // GET: api/Users/username
        [HttpGet("{username}")]
        public async Task<ActionResult<UserDTO>> GetUser(string username)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                return NotFound();
            }

            UserDTO result = new UserDTO()
            {
                Id= user.Id,
                Username = user.Username,
                Password = user.Password,
                ContactPhone = user.ContactPhone,
                Email = user.Email,
                RoleId = user.RoleId,
                //Role=user.Role.Title
            };
            return Ok(result);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> PutUser(int id, UserDTO sent_user)
        {
            if (id != sent_user.Id)
            {
                return BadRequest();
            }

            User user = new User()
            {
                Id=sent_user.Id,
                Username=sent_user.Username,
                Password=sent_user.Password,
                Email=sent_user.Email,
                ContactPhone=sent_user.ContactPhone
            };
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
