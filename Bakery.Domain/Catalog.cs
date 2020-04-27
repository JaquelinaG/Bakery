using System.Collections.Generic;

namespace Bakery.Domain
{
    public class Catalog
    {
        public int Id { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
