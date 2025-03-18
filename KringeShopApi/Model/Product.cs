using System;
using System.Collections.Generic;

namespace KringeShopApi.Model;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string TypeId { get; set; } = null!;

    public decimal Price { get; set; }

    public string Count { get; set; } = null!;
}
