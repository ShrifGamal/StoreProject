using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Attributes;
using Store.APIs.Errors;
using Store.Core.DTOs.Products;
using Store.Core.Helper;
using Store.Core.ServicesContract;
using Store.Core.Specifications.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [ProducesResponseType(typeof(PaginationResponse<ProductDto>) , StatusCodes.Status200OK)]
        [HttpGet]
        [Cached(100)]
        //[Authorize]
        public async Task<ActionResult<PaginationResponse<ProductDto>>> GetAllProducts([FromQuery] ProductSpecParams productSpec)
        {
            var Result = await _productService.GetAllProductsAsync(productSpec);
            return Ok(Result);
        }

        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllBrands()
        {
            var Result = await _productService.GetAllBrandsAsync();
            return Ok(Result);
        }

        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllTypes()
        {
            var Result = await _productService.GetAllTypesAsync();
            return Ok(Result);
        }

        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int? id)
        {
            if (id == null) return BadRequest(new ApiErrorResponse(400)); 
            var Result = await _productService.GetProductByIdAsync(id.Value);
            if (Result == null) return NotFound(new ApiErrorResponse(404, $"Product With Id {id} Is Not Found At DB"));
            return Ok(Result);
        }
    }
}
