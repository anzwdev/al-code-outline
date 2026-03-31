using AnZwDev.AL.CodeAnalysis.Extensions;
using AnZwDev.AL.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories
{
    internal static class XmlPortFieldAttributeSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(XmlPortFieldAttributeSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            return XmlPortFieldNodeSymbolFactory.CreateSymbol(node, parentNode, ALSyntaxNodeKind.XmlPortFieldAttribute);
        }
    }
}
