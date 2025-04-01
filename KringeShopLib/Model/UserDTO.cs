using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KringeShopLib.Model
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int RoleId { get; set; }

        public string Email { get; set; } = null!;

        public string ContactPhone { get; set; } = null!;
    }
}
