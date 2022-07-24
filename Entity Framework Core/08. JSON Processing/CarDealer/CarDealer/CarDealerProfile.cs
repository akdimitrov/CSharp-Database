using System.Linq;
using AutoMapper;
using CarDealer.DTOs.Car;
using CarDealer.DTOs.Customer;
using CarDealer.DTOs.Part;
using CarDealer.DTOs.Sale;
using CarDealer.DTOs.Supplier;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Suppliers
            CreateMap<ImportSupplierDto, Supplier>();
            CreateMap<Supplier, ExportLocalSupplierDto>()
                .ForMember(x => x.PartsCount, y => y.MapFrom(s => s.Parts.Count));

            //Parts
            CreateMap<ImportPartDto, Part>();
            CreateMap<Part, ExportPartDto>()
                .ForMember(x => x.Price, y => y.MapFrom(s => s.Price.ToString("f2")));

            //Customers
            CreateMap<ImportCustomerDto, Customer>();
            CreateMap<Customer, ExportOrderedCustomerDto>();

            //Sales
            CreateMap<ImportSaleDto, Sale>();
            CreateMap<Sale, ExportSaleWithDiscoutDto>()
                .ForMember(x => x.Discount, y => y.MapFrom(s => s.Discount.ToString("f2")))
                .ForMember(x => x.Price, y => y.MapFrom(s => s.Car.PartCars.Sum(x => x.Part.Price).ToString("f2")))
                .ForMember(x => x.PriceWithDiscount, y => y.MapFrom(s => (s.Car.PartCars.Sum(x => x.Part.Price) * (1 - (s.Discount / 100m))).ToString("f2")));

            //Cars
            CreateMap<Car, ExportToyotaCarDto>();
            CreateMap<Car, ExportCarDto>();
            CreateMap<Car, ExportCarWithPartsDto>()
                .ForMember(x => x.Car, y => y.MapFrom(s => Mapper.Map<ExportCarDto>(s)))
                .ForMember(x => x.Parts, y => y.MapFrom(s => Mapper.Map<ExportPartDto[]>(s.PartCars.Select(p => p.Part))));
        }
    }
}
