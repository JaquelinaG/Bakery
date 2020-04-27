using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Bakery.Domain;

namespace Bakery.Test
{
    [TestClass]
    public class OrderEntity
    {
        private ICatalogFactory catalogFactory;
        private Catalog catalog;

        [TestInitialize]
        public void Initialize()
        {
            this.catalogFactory = Mock.Of<ICatalogFactory>();

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
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InstantiatingASingleOrderWithNullDescription()
        {
            var singleOrder = new SingleOrder(this.catalogFactory, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InstantiatingASingleOrderWithEmptyDescription()
        {
            var singleOrder = new SingleOrder(this.catalogFactory, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InstantiatingASingleOrderWithWhiteSpace()
        {
            var singleOrder = new SingleOrder(this.catalogFactory, " ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InstantiatingASingleOrderWithNoWhiteSpaceInDescription()
        {
            var singleOrder = new SingleOrder(this.catalogFactory, "10VS5");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InstantiatingASingleOrderWithNoNumberInDescription()
        {
            var singleOrder = new SingleOrder(this.catalogFactory, "10A VS5");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InstantiatingASingleOrderWithWrongCodeInInDescription()
        {
            var singleOrder = new SingleOrder(this.catalogFactory, "10 VS6");
        }

        [TestMethod]
        public void InstantiatingASingleOrderWithValidDescription()
        {
            Mock.Get(this.catalogFactory).Setup(f => f.CreateCatalog()).Returns(this.catalog);
            var singleOrder = new SingleOrder(this.catalogFactory, "10 VS5");

            Assert.IsTrue(singleOrder.ProductUnits != 0);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(singleOrder.ProductCode));
        }
    }
}
