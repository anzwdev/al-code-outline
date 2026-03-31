using AnZwDev.AL.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories
{
    internal static class ApplicationObjectExtensionSymbolFactory
    {

        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(ApplicationObjectExtensionSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode, ALSyntaxNodeKind kind)
        {
            var symbol = ObjectSymbolFactory.CreateSymbol(node, node.ObjectId, parentNode, kind);

            if (node.BaseObject != null)
                symbol.Extends = node.BaseObject.ToString();

            return symbol;
        }


    }
}
