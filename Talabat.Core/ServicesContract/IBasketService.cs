using Store.Core.DTOs.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.ServicesContract
{
    public interface IBasketService
    {
        Task<CustomerBasketDto> GetBasketAsync(string BasketId);
        Task<CustomerBasketDto?> UpdateBasketAsync(CustomerBasketDto? basketDto);
        Task<bool> DeleteBasketAsync(string BasketId);
    }
}
