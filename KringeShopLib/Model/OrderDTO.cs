using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KringeShopLib.Model
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public string? Adress { get; set; }

        public decimal FullCost { get; set; }

        public DateTime? RecieveDate { get; set; }

        public int StatusId { get; set; }

        public string? Status { get; set; } = null;

        public DateTime? CreateDate { get; set; }

        public bool IsCmpleted { get; set; }

        public bool IsSelfPickUp { get; set; }

        public int UserId { get; set; }
    }
}
