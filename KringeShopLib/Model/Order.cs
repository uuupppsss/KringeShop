using System;
using System.Collections.Generic;

namespace KringeShopLib.Model;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Adress { get; set; } = null!;

    public decimal FullCost { get; set; }

    public DateTime? RecieveDate { get; set; }

    public int StatusId { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool IsCmpleted { get; set; }

    public virtual ICollection<ProductsInOrderList> ProductsInOrderLists { get; set; } = new List<ProductsInOrderList>();

    public virtual OrderStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
