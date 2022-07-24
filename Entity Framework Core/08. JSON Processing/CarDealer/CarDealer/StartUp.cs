using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Car;
using CarDealer.DTOs.Customer;
using CarDealer.DTOs.Part;
using CarDealer.DTOs.Sale;
using CarDealer.DTOs.Supplier;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new CarDealerContext();
            Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());

            // ---Task 01.Import Suppliers
            //string suppliersJson = File.ReadAllText("../../../Datasets/suppliers.json");
            //Console.WriteLine(ImportSuppliers(context, suppliersJson));

            // ---Task 02.Import Parts
            //string partsJson = File.ReadAllText("../../../Datasets/parts.json");
            //Console.WriteLine(ImportParts(context, partsJson));

            // ---Task 03.Import Cars
            //string carsJson = File.ReadAllText("../../../Datasets/cars.json");
            //Console.WriteLine(ImportCars(context, carsJson));

            // ---Task 04.Import Customers
            //string customersJson = File.ReadAllText("../../../Datasets/customers.json");
            //Console.WriteLine(ImportCustomers(context, customersJson));

            // ---Task 05.Import Sales
            //string salesJson = File.ReadAllText("../../../Datasets/sales.json");
            //Console.WriteLine(ImportSales(context, salesJson));

            // ---Task 06.Export Ordered Customers
            //Console.WriteLine(GetOrderedCustomers(context));

            // ---Task 07.Export Cars from Make Toyota
            //Console.WriteLine(GetCarsFromMakeToyota(context));

            // ---Task 08 .Export Local Suppliers
            //Console.WriteLine(GetLocalSuppliers(context));

            // ---Task 09. Export Cars with Their List of Parts
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));

            // ---Task 10. Export Total Sales by Customer
            //Console.WriteLine(GetTotalSalesByCustomer(context));

            // ---Task 13. Export Sales with Applied Discount
            Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var supplierDtos = JsonConvert.DeserializeObject<ImportSupplierDto[]>(inputJson);

            var inputSuppliers = Mapper.Map<Supplier[]>(supplierDtos);
            context.Suppliers.AddRange(inputSuppliers);
            context.SaveChanges();

            return $"Successfully imported {inputSuppliers.Count()}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var partDtos = JsonConvert.DeserializeObject<ImportPartDto[]>(inputJson);

            var inputParts = Mapper.Map<Part[]>(partDtos)
                .Where(x => context.Suppliers.Any(s => s.Id == x.SupplierId))
                .ToList();

            context.Parts.AddRange(inputParts);
            context.SaveChanges();

            return $"Successfully imported {inputParts.Count()}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carDtos = JsonConvert.DeserializeObject<ImportCarDto[]>(inputJson);

            var inputCars = new List<Car>();
            foreach (var carDto in carDtos)
            {
                Car car = new Car
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TravelledDistance
                };

                foreach (var partId in carDto.PartsId.Distinct())
                {
                    car.PartCars.Add(new PartCar
                    {
                        PartId = partId
                    });
                }

                inputCars.Add(car);
            }

            context.Cars.AddRange(inputCars);
            context.SaveChanges();

            return $"Successfully imported {inputCars.Count()}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customerDtos = JsonConvert.DeserializeObject<ImportCustomerDto[]>(inputJson);

            var inputCustomers = Mapper.Map<Customer[]>(customerDtos);
            context.Customers.AddRange(inputCustomers);
            context.SaveChanges();

            return $"Successfully imported {inputCustomers.Count()}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var saleDtos = JsonConvert.DeserializeObject<ImportSaleDto[]>(inputJson);

            var inputSales = Mapper.Map<Sale[]>(saleDtos);
            context.Sales.AddRange(inputSales);
            context.SaveChanges();

            return $"Successfully imported {inputSales.Count()}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .ProjectTo<ExportOrderedCustomerDto>()
                .ToList();

            var settings = new JsonSerializerSettings()
            {
                DateFormatString = "dd/MM/yyyy"
            };
            var customersJson = JsonConvert.SerializeObject(customers, Formatting.Indented, settings);
            File.WriteAllText("../../../ordered-customers.json", customersJson);

            return customersJson;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toyotaCars = context.Cars
                .Where(x => x.Make == "Toyota")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ProjectTo<ExportToyotaCarDto>()
                .ToList();

            var toyotaCarsJson = JsonConvert.SerializeObject(toyotaCars, Formatting.Indented);
            File.WriteAllText("../../../toyota-cars.json", toyotaCarsJson);

            return toyotaCarsJson;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var localSuppliers = context.Suppliers
                .Where(x => !x.IsImporter)
                .ProjectTo<ExportLocalSupplierDto>()
                .ToList();

            var localSuppliersJson = JsonConvert.SerializeObject(localSuppliers, Formatting.Indented);
            File.WriteAllText("../../../local-suppliers.json", localSuppliersJson);

            return localSuppliersJson;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .ProjectTo<ExportCarWithPartsDto>()
                .ToList();

            var carsAndPartsJson = JsonConvert.SerializeObject(cars, Formatting.Indented);
            File.WriteAllText("../../../cars-and-parts.json", carsAndPartsJson);

            return carsAndPartsJson;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customerDtos = context.Customers
                .Where(x => x.Sales.Any())
                .Select(x => new ExportTotalSalesByCustomerDto()
                {
                    FullName = x.Name,
                    BoughtCars = x.Sales.Count,
                    SpentMoney = x.Sales.Sum(y => y.Car.PartCars.Sum(x => x.Part.Price))
                })
                .OrderByDescending(x => x.SpentMoney)
                .ThenByDescending(x => x.BoughtCars)
                .ToArray();

            var customersJson = JsonConvert.SerializeObject(customerDtos, Formatting.Indented);
            File.WriteAllText("../../../customers-total-sales.json", customersJson);

            return customersJson;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .ProjectTo<ExportSaleWithDiscoutDto>()
                .ToList();

            var salesJson = JsonConvert.SerializeObject(sales, Formatting.Indented);
            File.WriteAllText("../../../sales-discounts.json", salesJson);

            return salesJson;
        }
    }
}
