using System.Collections.Generic;
using Bakery.Domain;

namespace Bakey.Data
{
    public class CatalogFactory : ICatalogFactory
    {
        public Catalog CreateCatalog()
        {
            return new Catalog()
            {
                Id = 1,
                Products = new List<Product>()
                {
                    new Product()
                    {
                        Id = 1,
                        Code = "VS5",
                        Name = "Vegemite Scroll",
                        Packs = new HashSet<Pack>()
                        {
                            new Pack() { Id = 1, Units = 3, Price = 6.99M },
                            new Pack() { Id = 2, Units = 5, Price = 8.99M }
                        }
                    },
                    new Product()
                    {
                        Id = 2,
                        Code = "MB11",
                        Name = "Blueberry Muffin",
                        Packs = new HashSet<Pack>()
                        {
                            new Pack() { Id = 11, Units = 2, Price = 9.95M },
                            new Pack() { Id = 12, Units = 5, Price = 16.95M },
                            new Pack() { Id = 13, Units = 8, Price = 24.95M }
                        }
                    },
                    new Product()
                    {
                        Id = 3,
                        Code = "CF",
                        Name = "Croissant",
                        Packs = new HashSet<Pack>()
                        {
                            new Pack() { Id = 21, Units = 3, Price = 5.95M },
                            new Pack() { Id = 22, Units = 5, Price = 9.95M },
                            new Pack() { Id = 23, Units = 9, Price = 16.99M }
                        }
                    }
                }
            };
        }
    }
}
