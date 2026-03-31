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
    internal static class ReportLayoutCompiler
    {

        public static List<ReportLayoutSymbol>? Compile(ReportRenderingSectionSyntax? syntax)
        {
            if (syntax == null)
                return null;
            return Compile(syntax.Layouts);
        }

        public static List<ReportLayoutSymbol> Compile(SyntaxList<ReportLayoutSyntax> syntaxList)
        {
            var list = new List<ReportLayoutSymbol>(syntaxList.Count);

            for (int i = 0; i < syntaxList.Count; i++)
            {
                var layout = Compile(syntaxList[i]);
                if (layout != null)
                    list.Add(layout);
            }
            return list;
        }

        public static ReportLayoutSymbol? Compile(ReportLayoutSyntax syntax)
        {
            if (syntax == null)
                return null;

            return new ReportLayoutSymbol()
            { 
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList)
            };
        }

    }
}
