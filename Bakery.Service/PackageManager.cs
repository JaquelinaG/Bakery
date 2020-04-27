using System;
using System.Collections.Generic;
using System.Linq;
using Bakery.Domain;

namespace Bakery.Service
{
    public class PackageManager : IPackageManager
    {
        private Catalog catalog;

        public PackageManager(ICatalogFactory catalogFactory)
        {
            this.catalog = catalogFactory.CreateCatalog();
        }

        public List<ProductSellingPack> MakePackages(Order order)
        {
            if (order == null)
            {
                return null;
            }
            
            var listP = new List<ProductSellingPack>();

            foreach (var so in order.SingleOrders)
            {
                var subList = this.MakeProductPackage(so);

                if (subList.Any())
                {
                    listP.Add(new ProductSellingPack()
                    {
                        ProductCode = so.ProductCode,
                        ProductTotalUnits = so.ProductUnits,
                        TotalCost = subList.Sum(i => i.PackUnits * i.Pack.Price),
                        SellingPacks = subList
                    });
                }
                else
                {
                    throw new ArgumentException($"The order for product {so.ProductCode} can not be prepared");
                }
            }

            return listP;
        }

        private List<SellingPack> MakeProductPackage(SingleOrder so)
        {
            var sellingList = new List<SellingPack>();

            var product = this.catalog.Products.First(p => p.Code == so.ProductCode);
            var packs = product.Packs.OrderByDescending(p => p.Units).ToArray();
            Pack[] clonedPacks;
            int rest = 1;

            for (int x = packs.Length; x >= 0; x--)
            {
                if (x == packs.Length)
                {
                    clonedPacks = packs.ToArray();
                }
                else
                {
                    clonedPacks = packs.ToArray().Where(p => p.Id != packs[x].Id).ToArray();
                }

                rest = this.MakePackageAndReturnRest(clonedPacks, so, sellingList);
                if (rest == 0)
                {
                    break;
                }
                else
                {
                    sellingList = sellingList.Where(p => p.ProductCode != product.Code).ToList();
                }
            }

            return sellingList;
        }

        private int MakePackageAndReturnRest(Pack[] packs, SingleOrder o, List<SellingPack> sellingList)
        {
            var units = o.ProductUnits;
            int rest = 1;

            for (int i = 0; i < packs.Length; i++)
            {
                var packUnits = packs[i].Units;
                if (units < packUnits)
                {
                    continue;
                }

                rest = this.AddPacksToListAndReturnRest(packs, o, sellingList, units, i, packUnits);

                if (rest == 0)
                {
                    break;
                }
                else
                {
                    units = units % packUnits;
                }
            }

            return rest;
        }

        private int AddPacksToListAndReturnRest(Pack[] packs, SingleOrder o, List<SellingPack> sellingList, int units, int i, int packUnits)
        {
            int rest;
            var div = units / packUnits;
            rest = units % packUnits;

            sellingList.Add(new SellingPack()
            {
                PackUnits = div,
                Pack = packs[i],
                ProductCode = o.ProductCode
            });

            return rest;
        }
    }
}
