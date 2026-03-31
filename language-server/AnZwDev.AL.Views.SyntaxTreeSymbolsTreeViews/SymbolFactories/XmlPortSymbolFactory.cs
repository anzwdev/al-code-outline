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
    internal static class XmlPortSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(XmlPortSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var symbol = ObjectSymbolFactory.CreateSymbol(node, node.ObjectId, parentNode, ALSyntaxNodeKind.XmlPortObject);
            symbol.Format = node.GetPropertyValue("Format")?.ToString()?.ToLower();
            return symbol;
        }
    }
}
