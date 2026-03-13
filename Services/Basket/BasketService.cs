using AutoMapper;
using Store.Core.DTOs.Baskets;
using Store.Core.Entites;
using Store.Core.RepositoriesContract;
using Store.Core.ServicesContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.Basket
{
    public class BasketService : IBasketService
    {
        private readonly IBasketcRepository _basketcRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketcRepository basketcRepository , IMapper mapper)
        {
            _basketcRepository = basketcRepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
            return await _basketcRepository.DeleteBasketAsync(BasketId);
        }

        public async Task<CustomerBasketDto> GetBasketAsync(string BasketId)
        {
            var basket = await _basketcRepository.GetBasketByIdAsync(BasketId);
            if (basket == null) return _mapper.Map<CustomerBasketDto>(new CustomerBasket() { Id = BasketId});
            return _mapper.Map<CustomerBasketDto>(basket);
        }

        public async Task<CustomerBasketDto?> UpdateBasketAsync(CustomerBasketDto? basketDto)
        {
            var basket = await _basketcRepository.UpdateBasketAsync(_mapper.Map<CustomerBasket>(basketDto));
            if(basket == null) return null;
            return _mapper.Map<CustomerBasketDto>(basket);
        }
    }
}
