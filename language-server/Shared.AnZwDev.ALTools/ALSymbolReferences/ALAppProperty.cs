using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppProperty : ALAppBaseElement
    {

        public string Value { get; set; }
        public string Name { get; set; }

        public ALAppProperty()
        {
        }

        public ALAppProperty(string newName, string newValue)
        {
            this.Name = newName;
            this.Value = newValue;
        }

        public bool Equals(string compareWith)
        {
            return (Value != null) && (Value.Equals(compareWith, StringComparison.OrdinalIgnoreCase));
        }

        public string GetStringValue()
        {
            if (Value != null)
                return ALSyntaxHelper.DecodeString(Value);
            return null;
        }

        public string GetNameValue()
        {
            if (Value != null)
                return ALSyntaxHelper.DecodeName(Value);
            return null;
        }

    }
}
