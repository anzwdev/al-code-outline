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
    internal static class XmlPortFieldElementSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(XmlPortFieldElementSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            return XmlPortFieldNodeSymbolFactory.CreateSymbol(node, parentNode, ALSyntaxNodeKind.XmlPortFieldElement);
        }
    }
}
