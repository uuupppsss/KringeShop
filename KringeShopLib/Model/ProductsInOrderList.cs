using System;
using System.Collections.Generic;

namespace KringeShopLib.Model;

public partial class ProductsInOrderList
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? OrderId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
