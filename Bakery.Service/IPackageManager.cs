using System.Collections.Generic;
using Bakery.Domain;

namespace Bakery.Service
{
    public interface IPackageManager
    {
        List<ProductSellingPack> MakePackages(Order order);
    }
}
