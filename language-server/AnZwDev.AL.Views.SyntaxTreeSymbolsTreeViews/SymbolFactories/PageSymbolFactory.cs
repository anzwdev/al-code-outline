using AnZwDev.AL.Syntax;
using AnZwDev.AL.CodeAnalysis.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories
{
    internal static class PageSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(PageSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var symbol = ObjectSymbolFactory.CreateSymbol(node, node.ObjectId, parentNode, ALSyntaxNodeKind.PageObject);

            symbol.Source = node.GetStringPropertyValue("SourceTable");
            symbol.Subtype = node.GetDecodedNamePropertyValue("PageType");

            return symbol;
        }
    }
}
