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
    internal static class QuerySymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(QuerySyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var symbol = ObjectSymbolFactory.CreateSymbol(node, node.ObjectId,parentNode, ALSyntaxNodeKind.QueryObject);

            var queryTypeValue = node.GetPropertyValue("QueryType");
            if (queryTypeValue != null)
                symbol.Subtype = ALLiteralParser.ParseName(queryTypeValue.ToString());

            return symbol;
        }
    }
}
