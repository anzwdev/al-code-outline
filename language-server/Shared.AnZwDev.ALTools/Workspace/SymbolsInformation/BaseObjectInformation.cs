using AnZwDev.ALTools.ALSymbolReferences;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class BaseObjectInformation : SymbolWithIdInformation
    {

        [JsonProperty("namespace", NullValueHandling = NullValueHandling.Ignore)]
        public string Namespace { get; set; }

        public BaseObjectInformation()
        {
        }

        public BaseObjectInformation(ALAppObject alAppObject) : base(alAppObject)
        {
            if (alAppObject.Properties != null)
                Caption = alAppObject.Properties.GetValue("Caption");
            Namespace = alAppObject.NamespaceName;
        }
    }
}
