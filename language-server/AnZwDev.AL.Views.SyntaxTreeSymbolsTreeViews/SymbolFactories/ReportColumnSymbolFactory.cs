using AnZwDev.AL.CodeAnalysis.Extensions;
using AnZwDev.AL.Syntax;
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
    internal static class ReportColumnSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(ReportColumnSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var symbol = SyntaxNodeSymbolFactory.CreateSymbol(
                node, node.Name, 
                ALSyntaxNodeKind.ReportColumn,
                TreeViewNodeNameSetters.KindWithIdentifierName,
                node.OpenBraceToken, node.CloseBraceToken);

            if (node.SourceExpression != null)
            {                
                var sourceExpression = node.SourceExpression.ToString();
                symbol.FullName = ALLiteralFormatter.GetName(symbol.Name) + ": " + sourceExpression;
                symbol.Source = sourceExpression;
            }

            return symbol;
        }
    }
}
