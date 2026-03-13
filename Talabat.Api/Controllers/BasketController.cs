using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Errors;
using Store.Core.DTOs.Baskets;
using Store.Core.Entites;
using Store.Core.RepositoriesContract;
using System.Threading.Tasks;

namespace Store.APIs.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketcRepository _basketcRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketcRepository basketcRepository , IMapper mapper)
        {
            _basketcRepository = basketcRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>>GetBasket(string id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(400, "Invalid Id !"));
            var basket = await _basketcRepository.GetBasketByIdAsync(id);
            if(basket is null) return new CustomerBasket() { Id = id};
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>>CreateOrUpdateBasket(CustomerBasketDto model)
        {
            var basket = await _basketcRepository.UpdateBasketAsync(_mapper.Map<CustomerBasket>(model));

            if (basket is null) return BadRequest(new ApiErrorResponse(400));
            return Ok(basket);
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
             await _basketcRepository.DeleteBasketAsync(id);
        }
    }
}
