using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("User")]
    public class ExportUserXmlDto
    {
        [XmlAttribute("username")]
        public string Username { get; set; }

        [XmlArray]
        public ExportPurchaseXmlDto[] Purchases { get; set; }

        [XmlElement]
        public decimal TotalSpent { get; set; }
    }
}
