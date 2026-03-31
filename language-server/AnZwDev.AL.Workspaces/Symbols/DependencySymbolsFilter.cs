using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Workspaces.Symbols
{
    public struct DependencySymbolsFilter
    {

        public HashSet<string>? ExcludedDependencies { get; set; }
        public HashSet<string>? IncludedDependencies { get; set; }
        public bool IncludeInaccessibleSymbols { get; set; }

    }
}
