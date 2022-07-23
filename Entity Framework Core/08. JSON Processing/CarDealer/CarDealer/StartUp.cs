using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Supplier;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        private static IMapper mapper;

        public static void Main(string[] args)
        {
            var contex = new CarDealerContext();
            var confing = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            mapper = confing.CreateMapper();

            string inputSuppliersJson = File.ReadAllText("../../../Datasets/suppliers.json");
            Console.WriteLine(ImportSuppliers(contex, inputSuppliersJson));

        }

        public static string ImportSuppliers(CarDealerContext context, string suppliers)
        {
            var supplierDtos = JsonConvert.DeserializeObject<ImportSupplierDto[]>(suppliers);

            var inputSuppliers = mapper.Map<Supplier[]>(supplierDtos);

            context.Suppliers.AddRange(inputSuppliers);

            context.SaveChanges();

            return $"Successfully imported {inputSuppliers.Count()}.";
        }
    }
}