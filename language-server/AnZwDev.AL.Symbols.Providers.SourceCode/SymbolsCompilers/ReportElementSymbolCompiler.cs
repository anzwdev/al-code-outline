using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class ReportElementSymbolCompiler
    {

        public static (List<ReportDataItemSymbol>?, List<ReportColumnSymbol>?) Compile(SyntaxList<ReportDataItemElementSyntax> syntax, HashSet<string>? usings, int indentation, string? owningDataItemName)
        {
            List<ReportDataItemSymbol>? dataItems = null;
            List<ReportColumnSymbol>? columns = null;

            for (int i=0; i<syntax.Count; i++)
            {
                switch (syntax[i])
                {
                    case ReportDataItemSyntax dataItemSyntax:
                        var dataItemSymbol = ReportDataItemSymbolCompiler.Compile(dataItemSyntax, usings, indentation, owningDataItemName);
                        if (dataItemSymbol != null)
                        {
                            if (dataItems == null)
                                dataItems = new List<ReportDataItemSymbol>();
                            dataItems.Add(dataItemSymbol);
                        }
                        break;
                    case ReportColumnSyntax reportColumnSyntax:
                        var columnSymbol = ReportColumnSymbolCompiler.Compile(reportColumnSyntax, owningDataItemName);
                        if (columnSymbol != null)
                        {
                            if (columns == null)
                                columns = new List<ReportColumnSymbol>();
                            columns.Add(columnSymbol);
                        }
                        break;
                }
            }

            return (dataItems, columns);
        }


    }
}
