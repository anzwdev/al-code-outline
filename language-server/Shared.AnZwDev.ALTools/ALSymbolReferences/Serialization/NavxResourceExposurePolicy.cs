using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AnZwDev.ALTools.ALSymbolReferences.Serialization
{
    public class NavxResourceExposurePolicy
    {

        [XmlAttribute("AllowDebugging")]
        public string AllowDebuggingText
        {
            get { return this.AllowDebugging.ToString(); }
            set { this.AllowDebugging = value.ToBool(); }
        }

        [XmlAttribute("AllowDownloadingSource")]
        public string AllowDownloadingSourceText
        {
            get { return this.AllowDownloadingSource.ToString(); }
            set { this.AllowDownloadingSource = value.ToBool(); }
        }

        [XmlAttribute("IncludeSourceInSymbolFile")]
        public string IncludeSourceInSymbolFileText
        {
            get { return this.IncludeSourceInSymbolFile.ToString(); }
            set { this.IncludeSourceInSymbolFile = value.ToBool(); }
        }

        [XmlIgnore]
        public bool AllowDebugging { get; set; }

        [XmlIgnore]
        public bool AllowDownloadingSource { get; set; }

        [XmlIgnore]
        public bool IncludeSourceInSymbolFile { get; set; }

    }
}
