using AnZwDev.System.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.TreeViewModel
{
    public struct SymbolHierarchyNodePropertyValue
    {

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        public SymbolHierarchyNodePropertyValue() : this(String.Empty, String.Empty)
        {
        }

        public SymbolHierarchyNodePropertyValue(string name, string value)
        {
            Name = name;
            Value = value.FirstLine().LimitLength(250);
        }

    }
}
