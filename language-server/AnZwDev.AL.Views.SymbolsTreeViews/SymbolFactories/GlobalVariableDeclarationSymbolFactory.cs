using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class GlobalVariableDeclarationSymbolFactory : GlobalVariableDeclarationSymbolFactory<GlobalVariableDeclarationSymbol>
    {
    }

    internal class GlobalVariableDeclarationSymbolFactory<T> : VariableDeclarationSymbolFactory<T> where T : GlobalVariableDeclarationSymbol
    {

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);

            if (symbol.Protected)
                node.Access = ALSyntaxNodeAccessModifier.Protected;

            return node;
        }

    }
}
