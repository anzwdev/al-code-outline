using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppDirective
    {

        public Range Range { get; set; }

        public ALAppDirective(Range range)
        {
            this.Range = range;
        }

    }
}
