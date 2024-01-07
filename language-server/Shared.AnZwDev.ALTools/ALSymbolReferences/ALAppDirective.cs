using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppDirective
    {

        public TextRange Range { get; set; }

        public ALAppDirective(TextRange range)
        {
            this.Range = range;
        }

    }
}
