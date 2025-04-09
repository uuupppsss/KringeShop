using System;
using System.Collections.Generic;

namespace KringeShopLib.Model;

public partial class SavedProduct
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
