using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Core.DTOs.Products;
using Store.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Mapping.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile(IConfiguration configuration)
        {
            CreateMap<Product , ProductDto>()
                .ForMember(P => P.BrandName , option => option.MapFrom(P => P.Brand.Name ))
                .ForMember(P => P.TypeName , option => option.MapFrom(P => P.Type.Name ))
                .ForMember(P => P.PictureUrl , option => option.MapFrom(new PictureUrlResolver(configuration)));

            CreateMap<ProductType, TypeBrandDto>();

            CreateMap<ProductBrand, TypeBrandDto>();
        }
    }
}
