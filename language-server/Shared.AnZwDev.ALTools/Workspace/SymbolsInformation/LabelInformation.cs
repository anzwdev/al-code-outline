using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class LabelInformation
    {

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        private string _propertyName;
        public LabelInformation(string propertyName)
        {
            this._propertyName = propertyName;
        }

        public LabelInformation(string propertyName, string newValue)
        {
            this._propertyName = propertyName;
            this.SetValue(newValue);
        }

        public void Update(ALAppPropertiesCollection propertiesCollection)
        {
            if (propertiesCollection != null)
            {
                ALAppProperty labelProperty = propertiesCollection.GetProperty(this._propertyName);
                if ((labelProperty != null) && (!String.IsNullOrWhiteSpace(labelProperty.Value)))
                {
                    this.Value = labelProperty.Value;
                    this.Comment = propertiesCollection.GetValue(this._propertyName + ".Comment");
                }
            }
        }

        public void SetValue(string newValue)
        {
            this.Value = newValue;
            this.Comment = null;
        }

        public void SetProperty(string name, string value)
        {
            name = name.ToLower();
            switch (name)
            {
                case "comment":
                    this.Comment = value;
                    break;
            }
        }

    }
}
