using AnZwDev.AL.CodeAnalysis.Extensions;
using AnZwDev.AL.Syntax;
using AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.Formatters;
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
    internal static class KeySymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(KeySyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode, bool hasTableKeys)
        {
            var symbol = SyntaxNodeSymbolFactory.CreateSymbol(
                node, node.Name, 
                ALSyntaxNodeKind.Key,
                TreeViewNodeNameSetters.IdentifierName,
                node.OpenBraceToken, node.CloseBraceToken);

            if (!hasTableKeys)
                symbol.Kind = ALSyntaxNodeKind.PrimaryKey;
            symbol.FullName = ALLiteralFormatter.GetName(symbol.Name) + ": " + FieldListFormatter.GetCode(node.Fields);

            return symbol;
        }
    }
}
