using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> _products;

        public ProductRepository()
        {
            _products = new List<Product>()
            {
                new Product { ProductId = 1, ProductName = "Bike", Quantity = 10, Price = 150 },
                new Product { ProductId = 2, ProductName = "Car", Quantity = 10, Price = 2500 },
            };
        }

        public Task AddProductAsync(Product product)
        {
            if (_products.Any(p => p.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }

            var maxId = _products.Max(p => p.ProductId);
            product.ProductId = maxId + 1;

            _products.Add(product);
            return Task.CompletedTask;
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await Task.FromResult(_products.First(p => p.ProductId == productId));
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await Task.FromResult(_products);
            }

            return _products.Where(p => p.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public Task UpdateProductAsync(Product product)
        {
            if (_products.Any(i => i.ProductId != product.ProductId &&
                i.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }

            var prodToUpdate = _products.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (prodToUpdate is not null)
            {
                prodToUpdate.ProductName = product.ProductName;
                prodToUpdate.Quantity = product.Quantity;
                prodToUpdate.Price = product.Price;
            }

            return Task.CompletedTask;
        }

        public Task DeleteProductByIdAsync(int productId)
        {
            var product = _products.FirstOrDefault(p => p.ProductId == productId);
            if (product is not null)
            {
                _products.Remove(product);
            }

            return Task.CompletedTask;
        }
    }
}
