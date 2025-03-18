﻿using System;
using System.Collections.Generic;

namespace KringeShopLib.Model;

public partial class Basket
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
