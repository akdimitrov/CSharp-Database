using System;

namespace CarDealer.DTOs.Customer
{
    public class ExportOrderedCustomerDto
    {
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public bool IsYoungDriver { get; set; }
    }
}
