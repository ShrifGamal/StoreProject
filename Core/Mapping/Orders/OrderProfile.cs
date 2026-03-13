using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Core.DTOs.Order;
using Store.Core.Entites.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Mapping.Orders
{
    public class OrderProfile  : Profile
    {
        public OrderProfile(IConfiguration configuration)
        {
            CreateMap<Order, OrderTorReturnDto>()
                     .ForMember(d => d.DeliveryMethod , options => options.MapFrom(s => s.DeliveryMethod.ShortName))
                     .ForMember(d => d.DeliveryMethodCost , options => options.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<OrderItem , OrderItemDto>()
                    .ForMember(d => d.ProductId, options => options.MapFrom(s => s.Product.ProductId))
                    .ForMember(d => d.ProductName, options => options.MapFrom(s => s.Product.ProductName))
                    .ForMember(d => d.PictureUrl, options => options.MapFrom(s => $"{configuration["BASEURL"]}{s.Product.PictureUrl}"));
        }
    }
}
