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
    internal static class ReportAddDataItemChangeCompiler
    {

        public static void Compile(ReportExtensionDataSetAddDataItemSyntax? syntax, HashSet<string>? usings, List<ReportDataItemSymbol> dataItemsList)
        {
            if (syntax != null)
            {
                var owningDataItemName = NameCompiler.Compile(syntax.Anchor);
                for (int i = 0; i < syntax.DataItems.Count; i++)
                {
                    var symbol = ReportDataItemSymbolCompiler.Compile(syntax.DataItems[i], usings, 0, owningDataItemName);
                    if (symbol != null)
                        dataItemsList.Add(symbol);
                }
            }
        }


    }
}
