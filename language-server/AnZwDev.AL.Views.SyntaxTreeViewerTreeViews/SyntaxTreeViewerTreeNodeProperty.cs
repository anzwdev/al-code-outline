using AnZwDev.System.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Views.SyntaxTreeViewerTreeViews
{
    public class SyntaxTreeViewerTreeNodeProperty
    {

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        public SyntaxTreeViewerTreeNodeProperty() : this(String.Empty, String.Empty)
        {
        }

        public SyntaxTreeViewerTreeNodeProperty(string name, string value)
        {
            Name = name;
            Value = value.FirstLine().LimitLength(250);
        }

    }
}
