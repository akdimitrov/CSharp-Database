using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("Game")]
    public class ExportGameXmlDto
    {
        [XmlAttribute("title")]
        public string Name { get; set; }

        [XmlElement]
        public string Genre { get; set; }

        [XmlElement]
        public decimal Price { get; set; }
    }
}