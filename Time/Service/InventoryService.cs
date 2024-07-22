using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stl.Fusion;
using Time.Data;

namespace Time.Service
{
    public class InventoryService
    {
        private readonly ConcurrentDictionary<string, Product> _products = new();
        private readonly List<PurchaseHistoryEntry> _purchaseHistory = new();

        public InventoryService()
        {
           
            _products["ProductA"] = new Product { Name = "ProductA", Stock = 10, Quantity = 1 };
            _products["ProductB"] = new Product { Name = "ProductB", Stock = 5, Quantity = 1 };
        }

        [ComputeMethod]
        public virtual Task<Product?> GetProductAsync(string name) =>
            Task.FromResult(_products.GetValueOrDefault(name));

        [ComputeMethod]
        public virtual Task<IEnumerable<Product>> GetProductsAsync() =>
            Task.FromResult(_products.Values.AsEnumerable());

        [ComputeMethod]
        public virtual Task<IEnumerable<PurchaseHistoryEntry>> GetPurchaseHistoryAsync() =>
            Task.FromResult(_purchaseHistory.AsEnumerable());

        public async Task<bool> PurchaseProductAsync(string name, int quantity, string buyerId)
        {
            if (_products.TryGetValue(name, out Product product))
            {
                lock (product)
                {
                    if (product.Stock >= quantity)
                    {
                        product.Stock -= quantity;
                        _purchaseHistory.Insert(0, new PurchaseHistoryEntry(name, buyerId, quantity, DateTime.Now));
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Error: Not enough stock for {name}. Available: {product.Stock}, Requested: {quantity}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Error: Product {name} not found.");
            }
            return false;
        }

        public async Task UpdateProductQuantityAsync(string name, int newQuantity)
        {
            if (_products.TryGetValue(name, out var product))
            {
                lock (product)
                {
                    product.Quantity = newQuantity;
                    if (product.Quantity < 1) product.Quantity = 1; 
                    if (product.Quantity > product.Stock) product.Quantity = product.Stock; 
                }
            }
        }
    }
}
