using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Ingredient : CodeTable
    {
        public string Notes { get; set; } = String.Empty;

        public Vendor PreferredVendor { get; set; }

        public long PreferredVendorId { get { return this.PreferredVendor == null ? 0 : this.PreferredVendor.Id; } }

        public Ingredient()
        {
            this.CreationDate = DateTime.Now;
            this.StartDate = DateTime.Today;
            this.EndDate = DateTime.MaxValue;

            this.PreferredVendor = new Vendor();
        }

        public Ingredient(string code, string name) : this()
        {
            this.Code = code;
            this.Description = name;
        }
    }
}
