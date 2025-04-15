using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KringeShopLib.Model
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int TypeId { get; set; }
        public string? Type {  get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }

        public int TimeBought { get; set; }

    }
}
