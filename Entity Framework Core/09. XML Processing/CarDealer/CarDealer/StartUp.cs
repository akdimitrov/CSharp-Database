using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new CarDealerContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            // ---Task 01. Import Suppliers
            //var suppliersXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(context, suppliersXml));

            // ---Task 02. Import Parts
            //var partsXml = File.ReadAllText("../../../Datasets/parts.xml");
            //Console.WriteLine(ImportParts(context, partsXml));

            // ---Task 03. Import Cars
            //var carsXml = File.ReadAllText("../../../Datasets/cars.xml");
            //Console.WriteLine(ImportCars(context, carsXml));

            // ---Task 04. Import Customers
            //var customersXml = File.ReadAllText("../../../Datasets/customers.xml");
            //Console.WriteLine(ImportCustomers(context, customersXml));

            // ---Task 05. Import Sales
            //var salesXml = File.ReadAllText("../../../Datasets/sales.xml");
            //Console.WriteLine(ImportSales(context, salesXml));

            // ---Task 06. Export Cars With Distance
            //Console.WriteLine(GetCarsWithDistance(context));

            // ---Task 07. Export Cars From Make BMW
            //Console.WriteLine(GetCarsFromMakeBmw(context));

            // ---Task 08. Export Local Suppliers
            //Console.WriteLine(GetLocalSuppliers(context));

            // ---Task 09. Export Cars With Their List Of Parts
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));

            // ---Task 10. Export Total Sales By Customer
            //Console.WriteLine(GetTotalSalesByCustomer(context));

            // ---Task 11. Export Sales With Applied Discount
            //Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportSupplierDto[]), new XmlRootAttribute("Suppliers"));
            var supplierDtos = (ImportSupplierDto[])serializer.Deserialize(new StringReader(inputXml));

            var mapper = InitializeMapper();
            var inputSuppliers = mapper.Map<Supplier[]>(supplierDtos);
            context.Suppliers.AddRange(inputSuppliers);
            context.SaveChanges();

            return $"Successfully imported {inputSuppliers.Count()}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportPartDto[]), new XmlRootAttribute("Parts"));
            var partDtos = (ImportPartDto[])serializer.Deserialize(new StringReader(inputXml));

            var supplierIds = context.Suppliers
                .Select(x => x.Id)
                .ToList();

            var mapper = InitializeMapper();
            var inputParts = mapper.Map<Part[]>(partDtos)
                .Where(x => supplierIds.Contains(x.SupplierId));

            context.Parts.AddRange(inputParts);
            context.SaveChanges();

            return $"Successfully imported {inputParts.Count()}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCarDto[]), new XmlRootAttribute("Cars"));
            var carDtos = (ImportCarDto[])serializer.Deserialize(new StringReader(inputXml));

            var partIds = context.Parts
                .Select(x => x.Id)
                .ToList();

            var mapper = InitializeMapper();
            var inputCars = new List<Car>();
            foreach (var carDto in carDtos)
            {
                var currentCar = mapper.Map<Car>(carDto);
                foreach (var partId in carDto.PartsId.Select(x => x.Id).Distinct()
                    .Where(x => partIds.Contains(x)))
                {
                    currentCar.PartCars.Add(new PartCar() { PartId = partId });
                }

                inputCars.Add(currentCar);
            }

            context.Cars.AddRange(inputCars);
            context.SaveChanges();

            return $"Successfully imported {inputCars.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCustomerDto[]), new XmlRootAttribute("Customers"));
            var customerDtos = (ImportCustomerDto[])serializer.Deserialize(new StringReader(inputXml));

            var mapper = InitializeMapper();
            var inputCustomers = mapper.Map<Customer[]>(customerDtos);
            context.Customers.AddRange(inputCustomers);
            context.SaveChanges();

            return $"Successfully imported {inputCustomers.Count()}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportSaleDto[]), new XmlRootAttribute("Sales"));
            var saleDtos = (ImportSaleDto[])serializer.Deserialize(new StringReader(inputXml));

            var carIds = context.Cars
                .Select(x => x.Id)
                .ToList();

            var mapper = InitializeMapper();
            var inputSales = mapper.Map<Sale[]>(saleDtos)
                .Where(x => carIds.Contains(x.CarId));
            context.Sales.AddRange(inputSales);
            context.SaveChanges();

            return $"Successfully imported {inputSales.Count()}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var maper = InitializeMapper();

            var cars = context.Cars
                .Where(x => x.TravelledDistance > 2_000_000)
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Take(10)
                .ProjectTo<ExportCarWithDistanceDto>(maper.ConfigurationProvider)
                .ToArray();

            return SerializeXml(cars, "cars");
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            IMapper mapper = InitializeMapper();

            var cars = context.Cars
                .Where(x => x.Make == "BMW")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ProjectTo<ExportBmwCarDto>(mapper.ConfigurationProvider)
                .ToArray();

            return SerializeXml(cars, "cars");
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            IMapper mapper = InitializeMapper();

            var suppliers = context.Suppliers
                .Where(x => !x.IsImporter)
                .ProjectTo<ExportLocalSupplierDto>(mapper.ConfigurationProvider)
                .ToArray();

            return SerializeXml(suppliers, "suppliers");
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            IMapper mapper = InitializeMapper();

            var carParts = context.Cars
                .ProjectTo<ExportCarWithPartsDto>(mapper.ConfigurationProvider)
                .OrderByDescending(x => x.TravelledDistance)
                .ThenBy(x => x.Model)
                .Take(5)
                .ToArray();

            return SerializeXml(carParts, "cars");
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            IMapper mapper = InitializeMapper();

            var customers = context.Customers
                .Where(x => x.Sales.Any())
                .ProjectTo<ExportCustomerTotalSalesDto>(mapper.ConfigurationProvider)
                .OrderByDescending(x => x.SpentMoney)
                .ToArray();

            return SerializeXml(customers, "customers");
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            IMapper mapper = InitializeMapper();

            var sales = context.Sales
                .ProjectTo<ExportSaleDto>(mapper.ConfigurationProvider)
                .ToArray();

            return SerializeXml(sales, "sales");
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

        private static IMapper InitializeMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());

            return config.CreateMapper();
        }
    }
}