using Store.Core.Entites;
using Store.Core.Entites.Order;
using Store.Repository.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repository.Data
{
    public static class StoreDbContextSeed
    {
        public async static Task SeedAsync(StoreDbContext _context)
        {
            //@"..\Store.Repository\Data\DataSeed\brands.json"

           if(_context.Brands.Count() == 0)
           {
                var brandData = File.ReadAllText(@"..\Store.Repository\Data\DataSeed\brands.json");

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                if (brands is not null && brands.Count() > 0)
                {
                    await _context.AddRangeAsync(brands);
                    await _context.SaveChangesAsync();
                }
           }

           if(_context.Types.Count() == 0)
           {
                var typeData = File.ReadAllText(@"..\Store.Repository\Data\DataSeed\types.json");

                var Type = JsonSerializer.Deserialize<List<ProductType>>(typeData);

                if(Type is not null && Type.Count() > 0)
                {
                    await _context.Types.AddRangeAsync(Type);
                    await _context.SaveChangesAsync();
                }

           }

           if(_context.Products.Count() == 0)
           {
                var productData =  File.ReadAllText(@"..\Store.Repository\Data\DataSeed\products.json");

                var product = JsonSerializer.Deserialize<List<Product>>(productData);

                if(product is not null && product.Count() > 0)
                {
                    await _context.Products.AddRangeAsync(product);
                    await _context.SaveChangesAsync();
                }

           }

           if(_context.DeliveryMethods.Count() == 0)
           {
                var deliveryData =  File.ReadAllText(@"..\Store.Repository\Data\DataSeed\delivery.json");

                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

                if(deliveryMethods is not null && deliveryMethods.Count() > 0)
                {
                    await _context.DeliveryMethods.AddRangeAsync(deliveryMethods);
                    await _context.SaveChangesAsync();
                }

           }



        }
    }
}
