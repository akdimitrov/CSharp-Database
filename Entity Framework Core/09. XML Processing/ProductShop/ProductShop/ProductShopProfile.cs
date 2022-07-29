using System.Linq;
using AutoMapper;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<ImportUserDto, User>();
            CreateMap<ImportProductDto, Product>();
            CreateMap<ImportCategoryDto, Category>();
            CreateMap<ImportCategoryProductDto, CategoryProduct>();

            CreateMap<Product, ExportProductInRangeDto>()
                .ForMember(x => x.BuyerFullName, y => y.MapFrom(s => s.BuyerId.HasValue ? $"{s.Buyer.FirstName} {s.Buyer.LastName}" : null));

            CreateMap<Product, ExportSoldProductDto>();
            CreateMap<User, ExportUserSoldProductsDto>();

            CreateMap<Category, ExportCategoryByProductsCount>()
                .ForMember(x => x.Count, y => y.MapFrom(s => s.CategoryProducts.Count()))
                .ForMember(x => x.AveragePrice, y => y.MapFrom(s => s.CategoryProducts.Average(z => z.Product.Price)))
                .ForMember(x => x.ToatalRevenue, y => y.MapFrom(s => s.CategoryProducts.Sum(z => z.Product.Price)));
        }
    }
}
