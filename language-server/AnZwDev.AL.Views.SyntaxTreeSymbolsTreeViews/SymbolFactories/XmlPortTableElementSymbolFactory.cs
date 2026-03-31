using AnZwDev.AL.CodeAnalysis.Extensions;
using AnZwDev.AL.Syntax;
using AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews;
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
    internal static class XmlPortTableElementSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(XmlPortTableElementSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var symbol = SyntaxNodeSymbolFactory.CreateSymbol(
                node, node.Name, 
                ALSyntaxNodeKind.XmlPortTableElement,
                TreeViewNodeNameSetters.KindWithIdentifierName,
                node.OpenBraceToken, node.CloseBraceToken);

            if (node.SourceTable != null)
            {
                var sourceTable = node.SourceTable.ToString();
                symbol.FullName = symbol.FullName +
                    ": Record " + sourceTable;
                symbol.Source = sourceTable;
            }

            return symbol;
        }
    }
}
