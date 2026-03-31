using AnZwDev.AL.Symbols;
using AnZwDev.System.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class ReportLabelSymbolCompiler
    {

        public static List<ReportLabelSymbol>? Compile(ReportLabelsSectionSyntax? syntax)
        {
            if (syntax == null)
                return null;

            return Compile(syntax.Labels);
        }

        public static List<ReportLabelSymbol> Compile(SyntaxList<ReportLabelBaseSyntax> syntaxList)
        {
            var list = new List<ReportLabelSymbol>(syntaxList.Count);
            for (int i = 0; i < syntaxList.Count; i++)
            {
                var symbol = Compile(syntaxList[i]);
                if (symbol != null)
                    list.Add(symbol);
            }
            return list;
        }

        public static ReportLabelSymbol? Compile(ReportLabelBaseSyntax? syntax)
        {
            if (syntax == null)
                return null;

            return new ReportLabelSymbol()
            {
                Id = 0,
                Name = NameCompiler.Compile(syntax.Name).NotNull()
            };
        }

    }
}
