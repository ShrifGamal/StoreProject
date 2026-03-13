using Microsoft.Extensions.Configuration;
using Store.Core;
using Store.Core.DTOs.Baskets;
using Store.Core.Entites;
using Store.Core.Entites.Order;
using Store.Core.ServicesContract;
using Store.Repository.Data.Configrations;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.Core.Entites.Product;

namespace Store.Service.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketService basketService , IUnitOfWork unitOfWork , IConfiguration configuration)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentIdAsync(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:Secret key"];

            var basket = await _basketService.GetBasketAsync(BasketId);
            if (basket == null) return null;

            var ShippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod , int>().GetByIdAsync(basket.DeliveryMethodId.Value);
                ShippingPrice = deliveryMethod.Cost;
            }

            if(basket.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(item.Id);
                    if(item.Price != product.Price)
                    {
                        item.Price = product.Price;
                    }
                }
            }

            var subTotal = basket.Items.Sum(I => I.Price * I.Quantity);

            var service = new PaymentIntentService();

            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(subTotal * 100 + ShippingPrice * 100),
                    PaymentMethodTypes = new List<string>() { "card" },
                    Currency = "USD"
                };

                paymentIntent = await service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(subTotal * 100 + ShippingPrice * 100),
                    
                };

                paymentIntent = await service.UpdateAsync( basket.PaymentIntentId,options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }

            basket = await _basketService.UpdateBasketAsync(basket);
            if (basket is null) return null;

            return basket;

        }
    }
}
