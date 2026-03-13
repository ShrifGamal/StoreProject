using AutoMapper;
using Store.Core;
using Store.Core.DTOs.Products;
using Store.Core.Entites;
using Store.Core.Helper;
using Store.Core.ServicesContract;
using Store.Core.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync()
        {
            var AllBrands = await _unitOfWork.Repository<ProductBrand , int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeBrandDto>>(AllBrands);
        }

        public async Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams productSpec)
        {
            var spec = new ProductSpecifications(productSpec);
            var AllProducts = await _unitOfWork.Repository<Product, int>().GetAllWithSpecAsync(spec);
            var MappedProduct = _mapper.Map<IEnumerable<ProductDto>>(AllProducts);
            var countspec = new ProductWithCountSpecification(productSpec);
            var count = await _unitOfWork.Repository<Product, int>().GetCountAsync(countspec);
            return  new PaginationResponse<ProductDto>(productSpec.PageSize , productSpec.PageIndex , count , MappedProduct);
                           
        }

        public async Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync()
        {
            var AllTypes = await _unitOfWork.Repository<ProductType , int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeBrandDto>>(AllTypes);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var spec = new ProductSpecifications(id);
            var GetProductById = await _unitOfWork.Repository<Product , int>().GetByIdWithSpecAsync(spec);
            return _mapper.Map<ProductDto>(GetProductById);
        }
    }
}
