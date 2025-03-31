using System;
using System.Collections.Generic;

namespace KringeShopApi.model;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Adress { get; set; }

    public decimal FullCost { get; set; }

    public DateTime? RecieveDate { get; set; }

    public int StatusId { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool IsCmpleted { get; set; }

    public bool IsSelfPickUp { get; set; }

    public virtual OrderStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
