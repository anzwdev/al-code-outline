using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AnZwDev.ALTools.ALSymbolReferences.Serialization
{
    [XmlRoot("Module")]
    public class NavxModuleReference
    {

        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Publisher")]
        public string Publisher { get; set; }

    }
}
