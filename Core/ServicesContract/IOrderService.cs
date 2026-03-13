using Store.Core.Entites.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.ServicesContract
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address ShippingAddress);
        Task<IEnumerable<Order>?> GetOrderForSpecificUserAsync(string buyerEmail);
        Task<Order>? GetOrderByIdForSpecificUserAsync(string buyerEmail , int OrderId);
    }
}
