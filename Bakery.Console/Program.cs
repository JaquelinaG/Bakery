namespace Bakery.Console
{
    using Bakery.Domain;
    using Bakery.Service;
    using Bakey.Data;
    using System;
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            var catalogFactory = new CatalogFactory();

            var customerOrder = new Order
            {
                SingleOrders = new HashSet<SingleOrder>()
                {
                    new SingleOrder(catalogFactory, "10 VS5"),
                    new SingleOrder(catalogFactory, "14 MB11"),
                    new SingleOrder(catalogFactory, "13 CF")
                }
            };

            var service = new PackageManager(catalogFactory);

            try
            {
                var result = service.MakePackages(customerOrder);
                foreach (var product in result)
                {
                    Console.WriteLine($"{product.ProductTotalUnits} {product.ProductCode} ${product.TotalCost} ");

                    foreach (var pack in product.SellingPacks)
                    {
                        Console.WriteLine($"{pack.PackUnits} x {pack.Pack.Units} ${pack.Pack.Price} ");
                    }
                }

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
    }
}
