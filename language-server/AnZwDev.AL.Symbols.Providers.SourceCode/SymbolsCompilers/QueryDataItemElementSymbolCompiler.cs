using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class QueryDataItemElementSymbolCompiler
    {

        public static (List<QueryDataItemSymbol>, List<QueryColumnSymbol>, List<QueryColumnSymbol>) Compile(SyntaxList<QueryDataItemElementSyntax> syntax, HashSet<string>? usings)
        {
            var dataItems = new List<QueryDataItemSymbol>();
            var columns = new List<QueryColumnSymbol>();
            var filters = new List<QueryColumnSymbol>();

            for (int i=0; i < syntax.Count; i++)
                Compile(syntax[i], usings, dataItems, columns, filters);

            return (dataItems, columns, filters);
        }

        private static void Compile(QueryDataItemElementSyntax syntax, HashSet<string>? usings, List<QueryDataItemSymbol> dataItems, List<QueryColumnSymbol> columns, List<QueryColumnSymbol> filters)
        {
            switch (syntax)
            {
                case QueryDataItemSyntax dataItemSyntax:
                    var dataItem = QueryDataItemSymbolCompiler.Compile(dataItemSyntax, usings);
                    if (dataItem != null)
                        dataItems.Add(dataItem);
                    break;
                case QueryColumnSyntax columnSyntax:
                    var column = QueryColumnSymbolCompiler.Compile(columnSyntax);
                    if (column != null)
                        columns.Add(column);
                    break;
                case QueryFilterSyntax filterSyntax:
                    var filter = QueryColumnSymbolCompiler.Compile(filterSyntax);
                    if (filter != null)
                        filters.Add(filter);
                    break;
            }
        }

    }
}
