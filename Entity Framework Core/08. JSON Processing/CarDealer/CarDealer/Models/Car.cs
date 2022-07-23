﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CarDealer.Models
{
    public class Car
    {
        public Car()
        {
            PartCars = new HashSet<PartCar>();
            Sales = new HashSet<Sale>();
        }

        public int Id { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        public long TravelledDistance { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }

        public virtual ICollection<PartCar> PartCars { get; set; }
    }
}