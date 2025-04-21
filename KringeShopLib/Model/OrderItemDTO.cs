using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KringeShopLib.Model
{
    public class OrderItemDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Count { get; set; }

        public int OrdeId { get; set; }

        public decimal Cost { get; set; }
    }
}
