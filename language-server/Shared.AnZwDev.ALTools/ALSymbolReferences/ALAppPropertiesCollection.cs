using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPropertiesCollection : List<ALAppProperty>
    {

        public ALAppPropertiesCollection()
        {
        }

        public ALAppProperty GetProperty(string name)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (name.Equals(this[i].Name, StringComparison.OrdinalIgnoreCase))
                    return this[i];
            }
            return null;
        }

        public string GetRawValue(string name)
        {
            var property = GetProperty(name);
            if (property != null)
                return property.Value;
            return null;
        }

        public string GetStringValue(string name)
        {
            var property = GetProperty(name);
            if (property != null)
                return property.GetStringValue();
            return null;
        }

        public string GetNameValue(string name)
        {
            var property = GetProperty(name);
            if (property != null)
                return property.GetNameValue();
            return null;
        }

        public ALAppProperty GetOrCreateProperty(string name)
        {
            ALAppProperty property = this.GetProperty(name);
            if (property == null)
            {
                property = new ALAppProperty(name, null);
                this.Add(property);
            }
            return property;
        }


    }
}
