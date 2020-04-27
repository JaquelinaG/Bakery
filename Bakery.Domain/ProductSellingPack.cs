using System.Collections.Generic;

namespace Bakery.Domain
{
    public class ProductSellingPack
    {
        public string ProductCode { get; set; }

        public int ProductTotalUnits { get; set; }

        public decimal TotalCost { get; set; }

        public List<SellingPack> SellingPacks { get; set; }
    }
}
