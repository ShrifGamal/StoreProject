using Store.Core;
using Store.Core.Entites;
using Store.Core.Entites.Order;
using Store.Core.ServicesContract;
using Store.Core.Specifications.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork , IBasketService basketService  ,IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _basketService = basketService;
            _paymentService = paymentService;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address ShippingAddress)
        {
            var basket = await _basketService.GetBasketAsync(basketId);
            if (basket == null) return null;
            var OrderItems = new List<OrderItem>();
            if (basket.Items.Count() > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(item.Id);
                    var ProductOrderItem = new ProductItemOrder(product.Id , product.Name , product.PictureUrl);
                    var orderItem = new OrderItem(ProductOrderItem, product.Price, item.Quantity);

                    OrderItems.Add(orderItem);
                }



            }

            

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(deliveryMethodId);

            var subTotal = OrderItems.Sum(I => I.Price * I.Quantity);

            if (! string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var spec = new OrderSpecificationWithPaymentIntentId(basket.PaymentIntentId);
                var ExOrder = await _unitOfWork.Repository<Order , int>().GetByIdWithSpecAsync(spec);
                _unitOfWork.Repository<Order, int>().Delete(ExOrder);
            }

            var basketDto = await _paymentService.CreateOrUpdatePaymentIntentIdAsync(basketId);

            var Order = new Order(buyerEmail , ShippingAddress , deliveryMethod , OrderItems , subTotal ,basketDto.PaymentIntentId);

            await _unitOfWork.Repository<Order , int>().AddAsync(Order);

            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return null;
            return Order;
        }

        public async Task<Order>? GetOrderByIdForSpecificUserAsync(string buyerEmail, int OrderId)
        {
            var spec = new OrderSpecifications(buyerEmail , OrderId);
            var order = await _unitOfWork.Repository<Order, int>().GetByIdWithSpecAsync(spec);
            if(order == null) return null;
            return order;
        }

        public async Task<IEnumerable<Order>?> GetOrderForSpecificUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);
            var order = await _unitOfWork.Repository<Order, int>().GetAllWithSpecAsync(spec);
            if(order == null) return null;
            return order;
        }
    }
}
