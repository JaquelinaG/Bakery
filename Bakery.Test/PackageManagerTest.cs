using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Bakery.Domain;
using Bakery.Service;

namespace Bakery.Test
{
    [TestClass]
    public class PackageManagerTest
    {
        private ICatalogFactory catalogFactory;
        private IPackageManager packageManager;
        private Catalog catalog;

        [TestInitialize]
        public void Initialize()
        {
            this.catalog = new Catalog()
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

            this.catalogFactory = Mock.Of<ICatalogFactory>();
            Mock.Get(this.catalogFactory).Setup(f => f.CreateCatalog()).Returns(this.catalog);

            this.packageManager = new PackageManager(this.catalogFactory);
        }

        [TestMethod]
        public void OrderExample_ShouldReturn2PacksOf5Units()
        {
            var customerOrder = new Order
            {
                SingleOrders = new HashSet<SingleOrder>()
                {
                    new SingleOrder(this.catalogFactory, "10 VS5")
                }
            };

            var result = this.packageManager.MakePackages(customerOrder);

            Assert.IsTrue(result.Count() == 1);
            Assert.IsTrue(result.All(p => p.ProductCode == "VS5" && p.ProductTotalUnits == 10 && p.TotalCost == 17.98M && p.SellingPacks.Count() == 1));
            Assert.IsTrue(result.All(p => p.SellingPacks.All(sp => sp.PackUnits == 2 && sp.Pack.Units == 5)));
        }

        [TestMethod]
        public void OrderExample_ShouldReturn1PackOf8UnitsAnd3PacksOf2Units()
        {
            var customerOrder = new Order
            {
                SingleOrders = new HashSet<SingleOrder>()
                {
                    new SingleOrder(this.catalogFactory, "14 MB11")
                }
            };

            var result = this.packageManager.MakePackages(customerOrder);

            Assert.IsTrue(result.Count() == 1);
            Assert.IsTrue(result.All(p => p.ProductCode == "MB11" && p.ProductTotalUnits == 14 && p.TotalCost == 54.80M && p.SellingPacks.Count() == 2));
            Assert.IsTrue(result.Any(p => p.SellingPacks.Any(sp => sp.PackUnits == 1 && sp.Pack.Units == 8)));
            Assert.IsTrue(result.Any(p => p.SellingPacks.Any(sp => sp.PackUnits == 3 && sp.Pack.Units == 2)));
        }

        [TestMethod]
        public void OrderExample_ShouldReturn2PacksOf5UnitsAnd1PackOf3Units()
        {
            var customerOrder = new Order
            {
                SingleOrders = new HashSet<SingleOrder>()
                {
                    new SingleOrder(this.catalogFactory, "13 CF")
                }
            };

            var result = this.packageManager.MakePackages(customerOrder);

            Assert.IsTrue(result.Count() == 1);
            Assert.IsTrue(result.All(p => p.ProductCode == "CF" && p.ProductTotalUnits == 13 && p.TotalCost == 25.85M && p.SellingPacks.Count() == 2));
            Assert.IsTrue(result.Any(p => p.SellingPacks.Any(sp => sp.PackUnits == 2 && sp.Pack.Units == 5)));
            Assert.IsTrue(result.Any(p => p.SellingPacks.Any(sp => sp.PackUnits == 1 && sp.Pack.Units == 3)));
        }

        [TestMethod]
        public void OrderExample_ShouldReturnAssignmentOutput()
        {
            var customerOrder = new Order
            {
                SingleOrders = new HashSet<SingleOrder>()
                {
                    new SingleOrder(this.catalogFactory, "10 VS5"),
                    new SingleOrder(this.catalogFactory, "14 MB11"),
                    new SingleOrder(this.catalogFactory, "13 CF")
                }
            };

            var result = this.packageManager.MakePackages(customerOrder);

            Assert.IsTrue(result.Count() == 3);
            Assert.IsTrue(result.Any(p => p.ProductCode == "VS5" && p.ProductTotalUnits == 10 && p.TotalCost == 17.98M && p.SellingPacks.Count() == 1));
            Assert.IsTrue(result.Any(p => p.ProductCode == "MB11" && p.ProductTotalUnits == 14 && p.TotalCost == 54.80M && p.SellingPacks.Count() == 2));
            Assert.IsTrue(result.Any(p => p.ProductCode == "CF" && p.ProductTotalUnits == 13 && p.TotalCost == 25.85M && p.SellingPacks.Count() == 2));
        }
    }
}
