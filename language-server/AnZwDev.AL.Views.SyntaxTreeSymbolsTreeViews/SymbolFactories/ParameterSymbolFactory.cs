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
    internal static class ParameterSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(ParameterSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var symbol = SyntaxNodeSymbolFactory.CreateSymbol(
                node, node.Name, 
                ALSyntaxNodeKind.Parameter,
                TreeViewNodeNameSetters.IdentifierName);

            if (node.Type != null)
                symbol.FullName = ALLiteralFormatter.GetName(symbol.Name) + ": " + DataTypeFormatter.GetCode(node.Type);
            return symbol;
        }
    }
}
