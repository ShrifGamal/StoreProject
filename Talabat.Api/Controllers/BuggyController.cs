using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Errors;
using Store.Repository.Data.Contexts;
using System.Threading.Tasks;

namespace Store.APIs.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly StoreDbContext _context;

        public BuggyController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("Not Found")]
        public async Task<IActionResult> GetNotFoundRequestErroe()
        {
            var brand = await _context.Brands.FindAsync(100);

            if(brand == null) return NotFound(new ApiErrorResponse(404));

            return Ok(brand);
        }

        [HttpGet("Server Error")]
        public async Task<IActionResult> GetServerError()
        {
            var brand = await _context.Brands.FindAsync(100);

            var brandToString = brand.ToString();

            return Ok(brand);
        }

        [HttpGet("Bad Request")]
        public async Task<IActionResult> GetBadRequestError()
        {
            return BadRequest(new ApiErrorResponse(400));
        }

        [HttpGet("Bad Request/{id}")]
        public async Task<IActionResult> GetBadRequestError(int id)
        {
            return Ok();
        }

        [HttpGet("Unauthorized")]
        public async Task<IActionResult> GetUnauthorizedError(int id)
        {
            return Unauthorized(new ApiErrorResponse(401));
        }



    }
}
