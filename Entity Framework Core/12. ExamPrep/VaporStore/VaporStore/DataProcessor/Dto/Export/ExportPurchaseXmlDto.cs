using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("Purchase")]
    public class ExportPurchaseXmlDto
    {
        [XmlElement("Card")]
        public string CardNumber { get; set; }

        [XmlElement]
        public string Cvc { get; set; }

        [XmlElement]
        public string Date { get; set; }

        [XmlElement]
        public ExportGameXmlDto Game { get; set; }
    }
}