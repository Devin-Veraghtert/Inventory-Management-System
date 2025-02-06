using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.EFCoreSqlServer
{
    public class ProductEFCoreRepository : IProductRepository
    {
        private readonly IDbContextFactory<IMSContext> contextFactory;

        public ProductEFCoreRepository(IDbContextFactory<IMSContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }
        public async Task AddProductAsync(Product product)
        {
            using var db = this.contextFactory.CreateDbContext();
            db.Products?.Add(product);

            FlagInventoryUnchanged(product, db);

            await db.SaveChangesAsync();
        }

        public async Task DeleteProductByIdAsync(int productId)
        {
            using var db = this.contextFactory.CreateDbContext();
            var product = db.Products?.Find(productId);
            if (product is null) return;

            db.Products?.Remove(product);
            await db.SaveChangesAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            using var db = this.contextFactory.CreateDbContext();
            var product = await db.Products.FindAsync(productId);
            if (product is not null) return product;

            return new Product();
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            using var db = this.contextFactory.CreateDbContext();
            return await db.Products.Where(p => p.ProductName.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            using var db = this.contextFactory.CreateDbContext();
            var pr = await db.Products.FindAsync(product.ProductId);
            if (pr is not null)
            {
                pr.ProductName = product.ProductName;
                pr.Price = product.Price;
                pr.Quantity = product.Quantity;
                pr.ProductInventories = product.ProductInventories;

                FlagInventoryUnchanged(product, db);

                await db.SaveChangesAsync();
            }
        }

        private void FlagInventoryUnchanged(Product product, IMSContext db)
        {
            if (product.ProductInventories is not null &&
                product.ProductInventories.Count() > 0)
            {
                foreach(var prodInv in product.ProductInventories)
                {
                    if(prodInv.Inventory is not null)
                    {
                        db.Entry(prodInv.Inventory).State = EntityState.Unchanged;
                    }
                }
            }
        }
    }
}
