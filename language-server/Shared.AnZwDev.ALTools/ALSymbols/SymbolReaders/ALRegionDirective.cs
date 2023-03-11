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
        public Range Range { get; set; }
        public Range SelectionRange { get; set; }
        public ALRegionDirective Next { get; set; }

        public ALRegionDirective() : this(true, 0, "", new Range(0, 0, 0, 0), new Range(0, 0, 0, 0))
        {
        }

        public ALRegionDirective(bool isStartRegion, int level, string name, Range range, Range selectionRange)
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
