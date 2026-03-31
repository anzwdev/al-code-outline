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
    internal static class EventDeclarationSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(EventDeclarationSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var symbol = SyntaxNodeSymbolFactory.CreateSymbol(
                node, node.Name, 
                ALSyntaxNodeKind.EventDeclaration,
                TreeViewNodeNameSetters.IdentifierName);
            
            string namePart = ParametersFormatter.GetCode(node.ParameterList);

            if (symbol.Name == null)
                symbol.Name = String.Empty;
            symbol.FullName = ALLiteralFormatter.GetName(symbol.Name) + namePart;

            return symbol;
        }
    }
}
