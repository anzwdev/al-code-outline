using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class EventSymbolFactory : EventSymbolFactory<EventSymbol>
    {
    }

    internal class EventSymbolFactory<T> : MethodSymbolFactory<T> where T : EventSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.EventDeclaration;
        }

    }
}
