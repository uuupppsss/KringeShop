using System;
using System.Collections.Generic;

namespace KringeShopApi.Model;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }
}
