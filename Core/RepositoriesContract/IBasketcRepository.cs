using Store.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.RepositoriesContract
{
    public interface IBasketcRepository
    {
        Task<CustomerBasket?>GetBasketByIdAsync(string BasketId);
        Task<CustomerBasket?>UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string BasketId);
    }
}
