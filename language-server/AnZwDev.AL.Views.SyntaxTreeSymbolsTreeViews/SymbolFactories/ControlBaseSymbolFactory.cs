using AnZwDev.AL.Syntax;
using AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.PropertySetters;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories
{
    internal static class ControlBaseSymbolFactory
    {

        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(ControlBaseSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode, ALSyntaxNodeKind kind)
        {
            var symbol = SyntaxNodeSymbolFactory.CreateSymbol(node, node.Name, 
                kind, 
                TreeViewNodeNameSetters.KindWithIdentifierName,
                node.OpenBraceToken, node.CloseBraceToken);
            return symbol;
        }


    }
}
