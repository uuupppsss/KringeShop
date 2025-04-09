﻿using System;
using System.Collections.Generic;

namespace KringeShopLib.Model;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int TypeId { get; set; }

    public decimal Price { get; set; }

    public int Count { get; set; }

    public int TimeBought { get; set; }

    public virtual ICollection<BasketItem> Basketitems { get; set; } = new List<BasketItem>();

    public virtual ICollection<OrderItem> Orderitems { get; set; } = new List<OrderItem>();

    public virtual ICollection<SavedProduct> Savedproducts { get; set; } = new List<SavedProduct>();

    public virtual ProductType Type { get; set; } = null!;
}
