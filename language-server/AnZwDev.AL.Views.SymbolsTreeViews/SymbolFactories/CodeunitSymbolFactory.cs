using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class CodeunitSymbolFactory : CodeunitSymbolFactory<CodeunitSymbol>
    {
    }

    internal class CodeunitSymbolFactory<T> : ObjectWithCodeSymbolFactory<T> where T : CodeunitSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.CodeunitObject;
        }

    }

}
