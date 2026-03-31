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
    internal static class MethodOrTriggerDeclarationSymbolFactory
    {

        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(MethodOrTriggerDeclarationSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode, ALSyntaxNodeKind kind)
        {
            var symbol = SyntaxNodeSymbolFactory.CreateSymbol(
                
                node, node.Name, 
                kind,
                TreeViewNodeNameSetters.KindWithIdentifierName);

            string namePart = ParametersFormatter.GetCode(node.ParameterList);

            if ((node.ReturnValue != null) && (node.ReturnValue.Kind != SyntaxKind.None))
                namePart = namePart + " " + ReturnValueFormatter.GetCode(node.ReturnValue);

            if (symbol.Name == null)
                symbol.Name = "";

            symbol.FullName = ALLiteralFormatter.GetName(symbol.Name) + namePart;

            return symbol;
        }


    }
}
