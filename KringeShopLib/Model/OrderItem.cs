using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KringeShopLib.Model
{
    public partial class OrderItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Count { get; set; }

        public int OrdeId { get; set; }

        public decimal Cost { get; set; }

        public virtual Order Orde { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;
    }
}
