using System.Collections.Generic;
using System.Linq;
using Stl.Fusion;

namespace Time.Data
{
    public class InventoryState 
    {
        public IEnumerable<Product> Products { get; set; }

        public InventoryState(IEnumerable<Product> products)
        {
            Products = products;
        }
    }
}
