using AnZwDev.ALTools.Core;
using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AnZwDev.ALTools.ALSymbolReferences.Serialization
{
    [XmlRoot("App")]
    public class NavxApp
    {

        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Publisher")]
        public string Publisher { get; set; }

        [XmlAttribute("Version")]
        public string VersionText 
        {
            get { return this.Version.Version; }
            set { this.Version.Version = value; }
        }

        [XmlAttribute("CompatibilityId")]
        public string CompatibilityId { get; set; }

        [XmlAttribute("Target")]
        public string Target { get; set; }

        [XmlAttribute("Runtime")]
        public string Runtime { get; set; }
        
        [XmlAttribute("PropagateDependencies")]
        public string PropagateDependenciesText 
        {
            get { return this.PropagateDependencies.ToString(); }
            set { this.PropagateDependencies = value.ToBool(); }
        }

        [XmlAttribute("ShowMyCode")]
        public string ShowMyCodeText 
        { 
            get { return this.ShowMyCode.ToString(); }
            set { this.ShowMyCode = value.ToBool(); }
        }

        [XmlIgnore]
        public bool ShowMyCode { get; set; }

        [XmlIgnore]
        public bool PropagateDependencies { get; set; }

        [XmlIgnore]
        public VersionNumber Version { get; set; }

        public NavxApp()
        {
            this.Version = new VersionNumber();
        }

        public bool Equals(NavxApp navxApp, bool compareVersion)
        {
            return
                (this.Id == navxApp.Id) &&
                (this.Name == navxApp.Name) &&
                (this.Publisher == navxApp.Publisher) &&
                ((!compareVersion) || (this.Version.Equal(navxApp.Version)));
        }

    }

}
