using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            // ---Task 01. Import Users
            //string usersXml = File.ReadAllText("../../../Datasets/users.xml");
            //Console.WriteLine(ImportUsers(context, usersXml));

            // ---Task 02. Import Products
            //string productsXml = File.ReadAllText("../../../Datasets/products.xml");
            //Console.WriteLine(ImportProducts(context, productsXml));

            // ---Task 03. Import Categories
            //string categoriesXml = File.ReadAllText("../../../Datasets/categories.xml");
            //Console.WriteLine(ImportCategories(context, categoriesXml));

            // ---Task 04. Import Categories and Products
            //string categoriesProductsXml = File.ReadAllText("../../../Datasets/categories-products.xml");
            //Console.WriteLine(ImportCategoryProducts(context, categoriesProductsXml));

            // ---Task 05. Export Products In Range
            //Console.WriteLine(GetProductsInRange(context));

            // ---Task 06. Export Sold Products
            //Console.WriteLine(GetSoldProducts(context));

            // ---Task 07. Export Categories By Products Count
            //Console.WriteLine(GetCategoriesByProductsCount(context));

            // ---Task 08. Export Users and Products
            Console.WriteLine(GetUsersWithProducts(context));
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var userDtos = DeserializeXml<ImportUserDto[]>(inputXml, "Users");

            IMapper mapper = InitializeMapper();
            var inputUsers = mapper.Map<User[]>(userDtos)
                .Where(x => IsValid(x))
                .ToArray();

            context.Users.AddRange(inputUsers);
            context.SaveChanges();

            return $"Successfully imported {inputUsers.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var productDtos = DeserializeXml<ImportProductDto[]>(inputXml, "Products");

            IMapper mapper = InitializeMapper();
            var inputProducts = mapper.Map<Product[]>(productDtos)
                .Where(x => IsValid(x))
                .ToArray();

            context.Products.AddRange(inputProducts);
            context.SaveChanges();

            return $"Successfully imported {inputProducts.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var categoryDtos = DeserializeXml<ImportCategoryDto[]>(inputXml, "Categories");

            IMapper mapper = InitializeMapper();
            var inputCategories = mapper.Map<Category[]>(categoryDtos)
                .Where(x => IsValid(x))
                .ToArray();

            context.Categories.AddRange(inputCategories);
            context.SaveChanges();

            return $"Successfully imported {inputCategories.Count()}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var categoryProductDtos = DeserializeXml<ImportCategoryProductDto[]>(inputXml, "CategoryProducts");

            var dbCategoryIds = context.Categories.Select(x => x.Id).ToList();
            var dbProductIds = context.Products.Select(x => x.Id).ToList();

            IMapper mapper = InitializeMapper();
            var inputCategoriesProducts = mapper.Map<CategoryProduct[]>(categoryProductDtos)
                .Where(x => dbCategoryIds.Contains(x.CategoryId) && dbProductIds.Contains(x.ProductId))
                .ToArray();

            context.CategoryProducts.AddRange(inputCategoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {inputCategoriesProducts.Count()}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            IMapper mapper = InitializeMapper();
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Take(10)
                .ProjectTo<ExportProductInRangeDto>(mapper.ConfigurationProvider)
                .ToArray();

            return SerializeXml(products, "Products");
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            IMapper mapper = InitializeMapper();
            var usersSoldProducts = context.Users
                .Where(x => x.ProductsSold.Any(x => x.BuyerId.HasValue))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Take(5)
                .ProjectTo<ExportUserSoldProductsDto>(mapper.ConfigurationProvider)
                .ToArray();

            return SerializeXml(usersSoldProducts, "Users");
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            IMapper mapper = InitializeMapper();
            var categories = context.Categories
                .ProjectTo<ExportCategoryByProductsCount>(mapper.ConfigurationProvider)
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.ToatalRevenue)
                .ToArray();

            return SerializeXml(categories, "Categories");
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var userProducts = new ExportUserAndProductsOuterDto()
            {
                Count = context.Users
                    .Count(x => x.ProductsSold.Any()),
                Users = context.Users
                    .Where(x => x.ProductsSold.Any())
                    .OrderByDescending(x => x.ProductsSold.Count())
                    //.ToArray()  - Judge only!
                    .Select(x => new ExportUserAndProductsDto()
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Age = x.Age,
                        SoldProducts = new ExportSoldProductsOuterDto()
                        {
                            Count = x.ProductsSold.Count(),
                            Products = x.ProductsSold.Select(x => new ExportSoldProductDto()
                            {
                                Name = x.Name,
                                Price = x.Price
                            })
                            .OrderByDescending(x => x.Price)
                            .ToArray()
                        }
                    })
                    .Take(10)
                    .ToArray()
            };


            return SerializeXml(userProducts, "Users");
        }

        private static string SerializeXml<T>(T dataTransferObjects, string xmlRootAttributeName)
        {
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var writer = new StringWriter();
            var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttributeName));
            serializer.Serialize(writer, dataTransferObjects, namespaces);

            return writer.ToString();
        }

        private static T DeserializeXml<T>(string inputXml, string xmlRootAttributeName)
        {
            var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttributeName));
            var dtos = (T)serializer.Deserialize(new StringReader(inputXml));

            return dtos;
        }

        private static IMapper InitializeMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());

            return config.CreateMapper();
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