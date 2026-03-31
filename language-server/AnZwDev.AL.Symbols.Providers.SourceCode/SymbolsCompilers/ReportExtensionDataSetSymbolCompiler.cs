using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.ReportLayoutChangesCompilers;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class ReportExtensionDataSetSymbolCompiler
    {

        public static (List<ReportDataItemSymbol>?, List<ReportColumnSymbol>?) Compile(ReportExtensionDataSetSectionSyntax? syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return (null, null);
            return Compile(syntax.Changes, usings);
        }

        private static (List<ReportDataItemSymbol>, List<ReportColumnSymbol>) Compile(SyntaxList<ReportExtensionDataSetChangeBaseSyntax> itemsList, HashSet<string>? usings)
        {
            var dataItemsList = new List<ReportDataItemSymbol>();
            var columnsList = new List<ReportColumnSymbol>();

            for (int i = 0; i < itemsList.Count; i++)
                Compile(itemsList[i], usings, dataItemsList, columnsList);

            return (dataItemsList, columnsList);
        }

        private static void Compile(ReportExtensionDataSetChangeBaseSyntax syntax, HashSet<string>? usings, List<ReportDataItemSymbol> dataItemsList, List<ReportColumnSymbol> columnsList)
        {
            switch (syntax)
            {
                case ReportExtensionDataSetAddDataItemSyntax addDataItemSyntax:
                    ReportAddDataItemChangeCompiler.Compile(addDataItemSyntax, usings, dataItemsList);
                    break;
                case ReportExtensionDataSetModifySyntax modifySyntax:                   
                    break;
                case ReportExtensionDataSetAddColumnSyntax addColumnSyntax:
                    ReportAddColumnChangeCompiler.Compile(addColumnSyntax, usings, columnsList);
                    break;
            }
        }

    }
}
