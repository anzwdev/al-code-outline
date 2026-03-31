using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Formatters;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class MethodParameterSymbolFactory : MethodParameterSymbolFactory<MethodParameterSymbol>
    {
    }

    internal class MethodParameterSymbolFactory<T> : VariableDeclarationSymbolFactory<T> where T : MethodParameterSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.Parameter;
        }

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);

            node.FullName = DisplayStringFormatter.FormatParameterSymbol(symbol);
            
            return node;
        }

    }
}
 