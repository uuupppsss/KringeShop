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
using KringeShopApi.HomeModel;

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
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            List<User> users = await _context.Users.ToListAsync();
            if (users is null || users.Count == 0) return NotFound();
            List<UserDTO> result = new List<UserDTO>();
            foreach (var user in users)
            {
                result.Add(new UserDTO()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Password = user.Password,
                    Email = user.Email,
                    ContactPhone = user.ContactPhone,
                    RoleId = user.RoleId,
                });
            }
            return Ok(result);
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
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                ContactPhone = user.ContactPhone,
                Email = user.Email,
                RoleId = user.RoleId,
                //Role=user.Role.Title
            };
            return Ok(result);
        }

        [HttpGet("ById/{user_id}")]
        public async Task<ActionResult<UserDTO>> GetUserData(int user_id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Id==user_id);
            if (user == null)
            {
                return NotFound();
            }

            UserDTO result = new UserDTO()
            {
                Id = user.Id,
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
        [HttpPut]
        public async Task<ActionResult> UpdateUser(UserDTO sent_user)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == sent_user.Id);
            if (user is null) return NotFound();
            user.Email = sent_user.Email;
            user.ContactPhone = sent_user.ContactPhone;
            user.Username = sent_user.Username;
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdatePassword/{user_id}")]
        public async Task<ActionResult> UpdateUserPassword(string new_password, int user_id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == user_id);
            if (user is null) return NotFound();
            user.Password = new_password;
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
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
