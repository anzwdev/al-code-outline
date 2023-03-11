using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class PropertyValue
    {

        public string name { get; set; }
        public string value { get; set; }

        public PropertyValue(string newName, string newValue)
        {
            //limit property value length
            if (newValue != null)
            {
                if (newValue.Length > 250)
                    newValue = newValue.Substring(0, 250);
                int pos = newValue.IndexOf('\n');
                if (pos >= 0)
                    newValue = newValue.Substring(0, pos);
            }

            this.name = newName;
            this.value = newValue;
        }
    }
}
