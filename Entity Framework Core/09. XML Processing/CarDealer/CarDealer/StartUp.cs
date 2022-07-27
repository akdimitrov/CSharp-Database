using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new CarDealerContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var suppliersXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            Console.WriteLine(ImportSuppliers(context, suppliersXml));

            var partsXml = File.ReadAllText("../../../Datasets/parts.xml");
            Console.WriteLine(ImportParts(context, partsXml));

            var carsXml = File.ReadAllText("../../../Datasets/cars.xml");
            Console.WriteLine(ImportCars(context, carsXml));

            var customersXml = File.ReadAllText("../../../Datasets/customers.xml");
            Console.WriteLine(ImportCustomers(context, customersXml));

            var salesXml = File.ReadAllText("../../../Datasets/sales.xml");
            Console.WriteLine(ImportSales(context, salesXml));
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

        private static IMapper InitializeMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());

            return config.CreateMapper();
        }
    }
}