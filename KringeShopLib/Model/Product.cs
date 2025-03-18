using System;
using System.Collections.Generic;

namespace KringeShopLib.Model;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int TypeId { get; set; }

    public decimal Price { get; set; }

    public string Count { get; set; } = null!;

    public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();

    public virtual ICollection<ProductsInOrderList> ProductsInOrderLists { get; set; } = new List<ProductsInOrderList>();

    public virtual ProductType Type { get; set; } = null!;
}
