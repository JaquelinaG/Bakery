using System.Collections.Generic;

namespace Bakery.Domain
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public HashSet<Pack> Packs { get; set; }
    }
}
