using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Footballers.DataProcessor.ImportDto
{
    public class ImportTeamJsonDto
    {
        [Required]
        [RegularExpression(@"^[A-Za-z0-9.\-\s]{3,40}$")]
        public string Name { get; set; }

        [Required]
        [MaxLength(40)]
        [MinLength(2)]
        public string Nationality { get; set; }

        [Required]
        [RegularExpression(@"^\d+$")]
        public string Trophies { get; set; }

        public int[] Footballers { get; set; }
    }
}
