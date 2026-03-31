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
    internal static class XmlPortFieldNodeSymbolFactory
    {

        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(XmlPortFieldNodeSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode, ALSyntaxNodeKind kind)
        {
            var symbol = SyntaxNodeSymbolFactory.CreateSymbol(
                node, node.Name, 
                ALSyntaxNodeKind.XmlPortFieldAttribute,
                TreeViewNodeNameSetters.KindWithIdentifierName,
                node.OpenBraceToken, node.CloseBraceToken);
            if (node.SourceField != null)
                symbol.Source = ALLiteralParser.ParseName(node.SourceField.ToString());
            return symbol;

        }

    }
}
