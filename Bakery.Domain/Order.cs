using System.Collections.Generic;

namespace Bakery.Domain
{
    public class Order
    {
        public HashSet<SingleOrder> SingleOrders { get; set; }
    }
}
