using System;
using System.Collections.Generic;

namespace KringeShopLib.Model;

public partial class ProductImage
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public byte[] Image { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
