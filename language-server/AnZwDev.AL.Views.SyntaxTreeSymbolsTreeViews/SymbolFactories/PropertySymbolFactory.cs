using AnZwDev.AL.CodeAnalysis.Extensions;
using AnZwDev.AL.Syntax;
using AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.PropertySetters;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories
{
    internal static class PropertySymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(PropertySyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var name = ALLiteralParser.ParseName(node.Name?.Identifier.Text) ?? String.Empty;
            var symbol = SyntaxNodeSymbolFactory.CreateSymbol(
                node, null,
                ALSyntaxNodeKind.Property,
                TreeViewNodeNameSetters.IdentifierName);

            symbol.Name = name;
            if (!String.IsNullOrWhiteSpace(symbol.Name))
                symbol.FullName = symbol.Kind.ToDescriptionString() + " " + symbol.Name;

            return symbol;
        }
    }
}
