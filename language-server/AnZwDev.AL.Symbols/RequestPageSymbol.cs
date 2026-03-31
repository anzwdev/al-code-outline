using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class RequestPageSymbol : Symbol
    {
        public required PropertySymbolsCollection? Properties { get; init; }
        public required List<GlobalVariableDeclarationSymbol>? Variables { get; init; }
        public required List<MethodSymbol>? Methods { get; init; }
        public required List<PageControlSymbol>? Controls { get; init; }
        public required List<PageActionSymbol>? Actions { get; init; }

    }
}
