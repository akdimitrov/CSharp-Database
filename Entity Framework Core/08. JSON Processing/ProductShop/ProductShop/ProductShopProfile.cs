using System.Linq;
using AutoMapper;
using ProductShop.DTOs.Categories;
using ProductShop.DTOs.CategoryProduct;
using ProductShop.DTOs.Products;
using ProductShop.DTOs.User;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //Users
            CreateMap<ImportUserDto, User>();
            CreateMap<User, ExportUserWithSoldProductsDto>()
                .ForMember(x => x.SoldProducts,
                    y => y.MapFrom(s =>
                        s.ProductsSold.Where(p => p.BuyerId.HasValue)));

            //Products
            CreateMap<ImportProductDto, Product>();
            CreateMap<Product, ExportProductInRangeDto>()
                .ForMember(x => x.SellerFullName, y => y.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));

            CreateMap<Product, ExportUserSoldProductsDto>();

            //Categories
            CreateMap<ImportCategoriesDto, Category>();
            CreateMap<Category, ExportCategoryByProductsCoutDto>()
                .ForMember(x => x.AveragePrice, y => y.MapFrom(s => s.CategoryProducts.Average(p => p.Product.Price).ToString("f2")))
                .ForMember(x => x.TotalRevenue, y => y.MapFrom(s => s.CategoryProducts.Sum(p => p.Product.Price).ToString("f2")));

            //CategoryProducts
            CreateMap<ImportCategoryProductDto, CategoryProduct>();
        }
    }
}
