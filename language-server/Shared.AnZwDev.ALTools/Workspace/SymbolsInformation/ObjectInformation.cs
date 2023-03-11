using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class ObjectInformation : BaseObjectInformation
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        public ObjectInformation()
        {
        }

        public ObjectInformation(ALAppObject alAppObject) : base(alAppObject)
        {
            this.Type = alAppObject.GetALSymbolKind().ToObjectTypeName();
        }

    }
}
