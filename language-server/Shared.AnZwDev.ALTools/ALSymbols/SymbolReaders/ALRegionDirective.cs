using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbols.SymbolReaders
{
    public class ALRegionDirective
    {

        public bool IsStartRegion { get; }
        public int Level { get; }
        public string Name { get; set; }
        public TextRange Range { get; set; }
        public TextRange SelectionRange { get; set; }
        public ALRegionDirective Next { get; set; }

        public ALRegionDirective() : this(true, 0, "", new TextRange(0, 0, 0, 0), new TextRange(0, 0, 0, 0))
        {
        }

        public ALRegionDirective(bool isStartRegion, int level, string name, TextRange range, TextRange selectionRange)
        {
            IsStartRegion = isStartRegion;
            Level = level;
            Name = name;
            Range = range;
            SelectionRange = selectionRange;
            Next = null;
        }

    }
}
