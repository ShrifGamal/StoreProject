using StackExchange.Redis;
using Store.Core.Entites;
using Store.Core.RepositoriesContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repository.Repositories
{
    public class BasketcRepository : IBasketcRepository
    {
        private readonly IDatabase _database;
        public BasketcRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
            return await _database.KeyDeleteAsync(BasketId);
        }

        public async Task<CustomerBasket?> GetBasketByIdAsync(string BasketId)
        {
            var basket = await _database.StringGetAsync(BasketId);

            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var createdOrUpdatedBasket = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
            if(createdOrUpdatedBasket is false)return null;
            return await GetBasketByIdAsync(basket.Id);
        }
    }
}
