using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AnZwDev.ALTools.ALSymbolReferences.Serialization
{

    [XmlRoot("Dependency")]
    public class NavxDependency
    {

        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Publisher")]
        public string Publisher { get; set; }

        [XmlAttribute("MinVersion")]
        public string MinVersion { get; set; }

        [XmlAttribute("CompatibilityId")]
        public string CompatibilityId { get; set; }

    }

}
