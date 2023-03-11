using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppAttribute : ALAppElementWithName
    {

        public List<ALAppAttributeArgument> Arguments { get; set; }

        public ALAppAttribute()
        {
        }

    }
}
