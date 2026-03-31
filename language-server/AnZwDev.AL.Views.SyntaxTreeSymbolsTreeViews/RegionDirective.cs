using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews
{
    internal class RegionDirective
    {

        public bool IsStartRegion { get; }
        public int Level { get; }
        public string Name { get; set; }
        public TextRange Range { get; set; }
        public TextRange SelectionRange { get; set; }
        public RegionDirective? Next { get; set; }

        public RegionDirective() : this(true, 0, "", new TextRange(0, 0, 0, 0), new TextRange(0, 0, 0, 0))
        {
        }

        public RegionDirective(bool isStartRegion, int level, string name, TextRange range, TextRange selectionRange)
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
