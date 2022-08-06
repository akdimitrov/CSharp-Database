using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Footballers.Data.Models.Enums;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class ImportFootballerXmlDto
    {
        [XmlElement("Name")]
        [Required]
        [MaxLength(40)]
        [MinLength(2)]
        public string Name { get; set; }

        [XmlElement("ContractStartDate")]
        [Required]
        public string ContractStartDate { get; set; }

        [XmlElement("ContractEndDate")]
        [Required]
        public string ContractEndDate { get; set; }

        [XmlElement("BestSkillType")]
        public int BestSkillType { get; set; }

        [XmlElement("PositionType")]
        public int PositionType { get; set; }
    }
}
