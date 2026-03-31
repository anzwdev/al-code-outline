using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols
{
    public class ProjectDefinitionSymbol : Symbol
    {

        public required ApplicationSymbol Application { get; init; }
        public List<ApplicationSymbol> Dependencies { get; } = new List<ApplicationSymbol>();

    }
}
