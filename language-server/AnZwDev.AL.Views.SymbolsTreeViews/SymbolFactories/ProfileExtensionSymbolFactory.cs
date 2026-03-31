using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class ProfileExtensionSymbolFactory : ProfileExtensionSymbolFactory<ProfileExtensionSymbol>
    {
    }

    internal class ProfileExtensionSymbolFactory<T> : ObjectExtensionSymbolFactory<T> where T : ProfileExtensionSymbol
    {
        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.ProfileExtensionObject;
        }
    }

}
