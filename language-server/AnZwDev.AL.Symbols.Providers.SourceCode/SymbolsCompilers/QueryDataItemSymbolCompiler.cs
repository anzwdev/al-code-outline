using AnZwDev.AL.Symbols;
using AnZwDev.System.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class QueryDataItemSymbolCompiler
    {

        public static List<QueryDataItemSymbol>? Compile(QueryElementsSyntax? syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;

            return Compile(syntax.DataItems, usings);    
        }

        private static List<QueryDataItemSymbol> Compile(SyntaxList<QueryDataItemSyntax> syntaxList, HashSet<string>? usings)
        {
            var list = new List<QueryDataItemSymbol>();
            foreach (var syntax in syntaxList)
            {
                var item = Compile(syntax, usings);
                if (item != null)
                    list.Add(item);
            }
            return list;
        }

        public static QueryDataItemSymbol? Compile(QueryDataItemSyntax syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;

            (var dataItems, var columns, var filters) = QueryDataItemElementSymbolCompiler.Compile(syntax.Elements, usings);

            return new QueryDataItemSymbol()
            {
                Id = 0,
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                RelatedTable = ObjectReferenceCompiler.Compile(ObjectKind.Table, usings, syntax.DataItemTable),
                DataItems = dataItems,
                Columns = columns,
                Filters = filters
            };

        }


    }
}
