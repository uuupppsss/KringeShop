using System;
using System.Collections.Generic;

namespace KringeShopLib.Model;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual UserRole Role { get; set; } = null!;

    public virtual ICollection<SavedProduct> SavedProducts { get; set; } = new List<SavedProduct>();
}
