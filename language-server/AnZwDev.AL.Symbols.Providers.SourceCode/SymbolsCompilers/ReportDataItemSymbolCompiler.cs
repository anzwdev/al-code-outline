using AnZwDev.AL.Symbols;
using AnZwDev.System.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class ReportDataItemSymbolCompiler
    {

        public static List<ReportDataItemSymbol>? Compile(ReportDataSetSectionSyntax? syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;
            return Compile(syntax.DataItems, usings, 0, null);
        }

        private static List<ReportDataItemSymbol>? Compile(SyntaxList<ReportDataItemSyntax> itemsList, HashSet<string>? usings, int indentation, string? owningDataItemName)
        {
            if (itemsList.Count == 0)
                return null;

            List<ReportDataItemSymbol> list = new List<ReportDataItemSymbol>();
            for (int i = 0; i < itemsList.Count; i++)
            {
                var symbol = Compile(itemsList[i], usings, indentation, owningDataItemName);
                if (symbol != null)
                    list.Add(symbol);
            }

            return list;
        }

        public static ReportDataItemSymbol? Compile(ReportDataItemSyntax? syntax, HashSet<string>? usings, int indentation, string? owningDataItemName)
        {
            if (syntax == null)
                return null;

            var name = NameCompiler.Compile(syntax.Name).NotNull();
            (var dataItems, var columns) = ReportElementSymbolCompiler.Compile(syntax.Elements, usings, indentation + 1, name);

            return new ReportDataItemSymbol()
            {
                Id = 0,
                Name = name,
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                Indentation = indentation,
                DataItems = dataItems,
                Columns = columns,
                RelatedTable = ObjectReferenceCompiler.Compile(ObjectKind.Table, usings, syntax.DataItemTable),

                //?????
                FilterControlId = 0,
                OwningDataItemName = null
            };

        }


    }
}
