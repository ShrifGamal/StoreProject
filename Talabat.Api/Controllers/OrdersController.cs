using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Errors;
using Store.Core;
using Store.Core.DTOs.Order;
using Store.Core.Entites.Order;
using Store.Core.ServicesContract;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Store.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService orderService , IMapper mapper , IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto model)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            var address = _mapper.Map<Address>(model.ShipToAddress);

            var orders = await  _orderService.CreateOrderAsync(userEmail, model.BasketId, model.DeliveryMethodId, address);

            if (orders is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var MappedOrder = _mapper.Map<OrderTorReturnDto>(orders);

            return Ok(MappedOrder);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetOrderForSpecificUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));


            var orders = await _orderService.GetOrderForSpecificUserAsync(userEmail);

            if (orders is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<IEnumerable<OrderTorReturnDto>>(orders));
        }

        [Authorize]
        [HttpGet("{OrderId}")]
        public async Task<IActionResult> GetOrderByIdForSpecificUser(int OrderId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));


            var order = await _orderService.GetOrderByIdForSpecificUserAsync(userEmail , OrderId);

            if (order is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));

            return Ok(_mapper.Map<OrderTorReturnDto>(order));
        }

        [HttpGet("DeliveryMethod")]
        public async Task<IActionResult> GetDeliveryMethod()
        {
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod , int>().GetAllAsync();
            if (deliveryMethod is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(deliveryMethod);
        }

    }
}
