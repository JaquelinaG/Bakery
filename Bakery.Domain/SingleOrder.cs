using System;
using System.Linq;

namespace Bakery.Domain
{
    public class SingleOrder
    {
        private readonly ICatalogFactory catalogFactory;

        public SingleOrder(ICatalogFactory catalogFactory, string description)
        {
            this.catalogFactory = catalogFactory;

            if (!this.IsValidDescription(description))
            {
                throw new ArgumentException("Invalid Description");
            }

            this.SetUnitsAndCode(description);
        }

        public int Id { get; set; }

        public string ProductCode { get; private set; }

        public int ProductUnits { get; private set; }

        public bool IsValidDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return false;
            }
            else if (!description.Contains(' '))
            {
                return false;
            }

            var units = this.GetUnitsFromDescription(description);
            if (!int.TryParse(units, out int number) || int.Parse(units) == 0)
            {
                return false;
            }

            var catalog = this.catalogFactory.CreateCatalog();
            var code = this.GetCodeFromDescription(description);
            if (catalog == null || !catalog.Products.Any(p => p.Code == code))
            {
                return false;
            }

            return true;            
        }

        private string GetUnitsFromDescription(string description)
        {
            return description.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries).First();
        }

        private string GetCodeFromDescription(string description)
        {
            return description.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries).Last();
        }        

        private void SetUnitsAndCode(string description)
        {
            this.ProductUnits = int.Parse(this.GetUnitsFromDescription(description));

            this.ProductCode = this.GetCodeFromDescription(description);
        }
    }
}