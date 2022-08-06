using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class ImportGameDto
    {
        [Required]
        public string Name { get; set; }

        [Range(0 , double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public DateTime? ReleaseDate { get; set; }

        [Required]
        public string Developer { get; set; }

        [Required]
        public string Genre { get; set; }

        [MinLength(1)]
        public IEnumerable<string> Tags { get; set; }
    }
}
