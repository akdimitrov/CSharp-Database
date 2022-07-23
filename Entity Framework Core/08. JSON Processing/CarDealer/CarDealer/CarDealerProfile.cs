using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CarDealer.DTOs.Supplier;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<ImportSupplierDto, Supplier>();
        }
    }
}
