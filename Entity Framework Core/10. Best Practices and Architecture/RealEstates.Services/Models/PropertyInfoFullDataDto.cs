using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RealEstates.Services.Models
{
    [XmlType("Property")]
    public class PropertyInfoFullDataDto
    {
        [XmlAttribute]
        public int Id { get; set; }

        [XmlElement]
        public string DistrictName { get; set; }

        [XmlElement]
        public int Size { get; set; }

        [XmlAttribute]
        public int Price { get; set; }

        [XmlElement]
        public string PropertyType { get; set; }

        [XmlElement]
        public string BuildingType { get; set; }

        [XmlElement]
        public int Year { get; set; }

        [XmlArray]
        public TagInfoDto[] Tags { get; set; }
    }
}
