using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.ReportLayoutChangesCompilers
{
    internal static class ReportAddColumnChangeCompiler
    {

        public static void Compile(ReportExtensionDataSetAddColumnSyntax syntax, HashSet<string>? usings, List<ReportColumnSymbol> columnsList)
        {
            if (syntax != null)
            {
                var owningDataItemName = NameCompiler.Compile(syntax.Anchor);
                for (int i = 0; i < syntax.Columns.Count; i++)
                {
                    var symbol = ReportColumnSymbolCompiler.Compile(syntax.Columns[i], owningDataItemName);
                    if (symbol != null)
                        columnsList.Add(symbol);
                }
            }
        }

    }
}
