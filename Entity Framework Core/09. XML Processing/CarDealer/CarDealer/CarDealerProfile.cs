using System.Linq;
using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<ImportSupplierDto, Supplier>();
            CreateMap<ImportPartDto, Part>();
            CreateMap<ImportCarDto, Car>();
            CreateMap<ImportCustomerDto, Customer>();
            CreateMap<ImportSaleDto, Sale>();

            CreateMap<Car, ExportCarWithDistanceDto>();
            CreateMap<Car, ExportBmwCarDto>();
            CreateMap<Supplier, ExportLocalSupplierDto>();
            CreateMap<Part, ExportPartDto>();

            CreateMap<Car, ExportCarWithPartsDto>()
                .ForMember(x => x.Parts, y => y.MapFrom(s => s.PartCars
                        .Select(x => x.Part)
                        .OrderByDescending(x => x.Price)));

            CreateMap<Customer, ExportCustomerTotalSalesDto>()
                .ForMember(x => x.SpentMoney, y => y.MapFrom(s => s.Sales
                        .Select(s => s.Car)
                        .SelectMany(c => c.PartCars)
                        .Sum(p => p.Part.Price)));

            CreateMap<Car, ExportCarDto>();
            CreateMap<Sale, ExportSaleDto>()
                .ForMember(x => x.Price, y => y.MapFrom(s =>
                        s.Car.PartCars.Sum(x => x.Part.Price)))
                .ForMember(x => x.PriceAfterDiscount, y => y.MapFrom(s =>
                        s.Car.PartCars.Sum(x => x.Part.Price) * (1 - (s.Discount / 100m))));
        }
    }
}
