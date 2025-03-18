using System;
using System.Collections.Generic;

namespace KringeShopLib.Model;

public partial class UserRole
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
