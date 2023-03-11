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

    }
}
