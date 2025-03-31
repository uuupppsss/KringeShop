﻿using System;
using System.Collections.Generic;

namespace KringeShopApi.model;

public partial class ProductType
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
