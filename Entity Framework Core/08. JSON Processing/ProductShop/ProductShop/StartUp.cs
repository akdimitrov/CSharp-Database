using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Categories;
using ProductShop.DTOs.CategoryProduct;
using ProductShop.DTOs.Products;
using ProductShop.DTOs.User;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();
            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            // ---Task 01. Import Products
            //string usersJson = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, usersJson));

            // ---Task 02. Import Products
            //string productsJson = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, productsJson));

            // ---Task 03. Import Categories
            //string categoriesJson = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, categoriesJson));

            // ---Task 04. Import Categories and Products
            //string categoriesProductsJson = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, categoriesProductsJson));

            // ---Task 05. Export Products in Range
            //Console.WriteLine(GetProductsInRange(context));

            // ---Task 06. Export Sold Products
            //Console.WriteLine(GetSoldProducts(context));

            // ---Task 07. Export Categories by Products Count
            //Console.WriteLine(GetCategoriesByProductsCount(context));

            // ---Task 08. Export Users and Products
            //Console.WriteLine(GetUsersWithProducts(context));
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);

            var inputUsers = Mapper.Map<User[]>(userDtos)
                .Where(x => IsValid(x));
            context.Users.AddRange(inputUsers);
            context.SaveChanges();

            return $"Successfully imported {inputUsers.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var productDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);

            var inputProducts = Mapper.Map<Product[]>(productDtos)
                .Where(x => IsValid(x));
            context.Products.AddRange(inputProducts);
            context.SaveChanges();

            return $"Successfully imported {inputProducts.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categoryDtos = JsonConvert.DeserializeObject<ImportCategoriesDto[]>(inputJson);

            var inputCategories = Mapper.Map<Category[]>(categoryDtos)
                .Where(x => IsValid(x));
            context.Categories.AddRange(inputCategories);
            context.SaveChanges();

            return $"Successfully imported {inputCategories.Count()}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProductDtos = JsonConvert.DeserializeObject<ImportCategoryProductDto[]>(inputJson);

            var inputCategoryProducts = Mapper.Map<CategoryProduct[]>(categoryProductDtos)
                .Where(x => IsValid(x));
            context.CategoryProducts.AddRange(inputCategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {inputCategoryProducts.Count()}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .ProjectTo<ExportProductInRangeDto>()
                .ToArray();

            var productsJson = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText("../../../products-in-range.json", productsJson);

            return productsJson;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var usersSoldProducts = context.Users
                .Where(x => x.ProductsSold.Any(p => p.BuyerId.HasValue))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ProjectTo<ExportUserWithSoldProductsDto>()
                .ToList();

            var usersSoldProductsJson = JsonConvert.SerializeObject(usersSoldProducts, Formatting.Indented);
            File.WriteAllText("../../../users-sold-products.json", usersSoldProductsJson);

            return usersSoldProductsJson;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(x => x.CategoryProducts.Count())
                .ProjectTo<ExportCategoryByProductsCoutDto>()
                .ToList();

            var categoriesJson = JsonConvert.SerializeObject(categories, Formatting.Indented);
            File.WriteAllText("../../../categories-by-products.json", categoriesJson);

            return categoriesJson;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            ExportUserWithFullProductInfoDto usersFullInfo = new ExportUserWithFullProductInfoDto()
            {
                Users = context.Users
                    .Where(x => x.ProductsSold.Any(p => p.BuyerId.HasValue))
                    .OrderByDescending(x => x.ProductsSold.Count(x => x.BuyerId.HasValue))
                    .Select(x => new ExportUserInfoDto()
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Age = x.Age,
                        ProductsSold = new ExportSoldProductFullInfoDto()
                        {
                            ProductsSold = x.ProductsSold
                                .Where(p => p.BuyerId.HasValue)
                                .Select(x => new ExportSoldProductInfoDto()
                                {
                                    Name = x.Name,
                                    Price = x.Price
                                })
                                .ToArray()
                        }
                    })
                    .ToArray()
            };

            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            var usersFullInfoJson = JsonConvert.SerializeObject(usersFullInfo, settings);
            File.WriteAllText("../../../users-and-products.json", usersFullInfoJson);

            return usersFullInfoJson;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}