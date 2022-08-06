using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Coach")]
    public class ExportCoachXmlDto
    {
        [XmlAttribute("FootballersCount")]
        public int FottballersCount { get; set; }

        [XmlElement("CoachName")]
        public string CoachName { get; set; }

        [XmlArray("Footballers")]
        public ExportFootballerXmlDto[] Footballers { get; set; }
    }
}
