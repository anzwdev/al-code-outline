using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.PageControlCompilers;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class PageControlSymbolCompiler
    {

        public static List<PageControlSymbol>? Compile(PageLayoutSyntax? syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;
            return Compile(syntax.Areas, usings);
        }

        public static List<PageControlSymbol>? Compile<T>(SyntaxList<T> controlsList, HashSet<string>? usings) where T : ControlBaseSyntax
        {
            if (controlsList.Count == 0)
                return null;

            List<PageControlSymbol> list = new List<PageControlSymbol>();
            for (int i = 0; i < controlsList.Count; i++)
            {
                var control = Compile(controlsList[i], usings);
                if (control != null)
                    list.Add(control);
            }

            return list;
        }

        public static List<PageControlSymbol> Compile(SeparatedSyntaxList<IdentifierNameSyntax> syntax)
        {
            if (syntax.Count == 0)
                return new List<PageControlSymbol>();

            var list = new List<PageControlSymbol>(syntax.Count);
            for (int i=0; i<syntax.Count; i++)
            {
                var item = Compile(syntax[i]);
                if (item != null)
                    list.Add(item);
            }
            return list;
        }


        private static PageControlSymbol? Compile(IdentifierNameSyntax? syntax)
        {
            var name = NameCompiler.Compile(syntax);
            if (name == null)
                return null;

            return new PageControlSymbol()
            {
                Name = name,
                Kind = PageControlKind.Undefined,
                Actions = null,
                Controls = null,
                Properties = null,
                Id = 0,
                RelatedControlAddIn = null,
                RelatedControlAddInPublicKey = null,
                TypeDefinition = null,
                RelatedPagePartId = null
            };
        }

        private static PageControlSymbol? Compile(ControlBaseSyntax syntax, HashSet<string>? usings)
        {
            switch (syntax)
            {
                case PageAreaSyntax areaSyntax:
                    return PageAreaCompiler.Compile(areaSyntax, usings);
                case PageGroupSyntax groupSyntax:
                    return PageGroupCompiler.Compile(groupSyntax, usings);
                case PageFieldSyntax fieldSyntax:
                    return PageFieldCompiler.Compile(fieldSyntax, usings);
                case PagePartSyntax partSyntax:
                    return PagePartCompiler.Compile(partSyntax, usings);
                case PageSystemPartSyntax systemPartSyntax:
                    return PageSystemPartCompiler.Compile(systemPartSyntax, usings);
                case PageChartPartSyntax chartPartSyntax:
                    return PageChartPartCompiler.Compile(chartPartSyntax, usings);
                case PageLabelSyntax labelSyntax:
                    return PageLabelCompiler.Compile(labelSyntax, usings);
                case PageUserControlSyntax userControlSyntax:
                    return PageUserControlCompiler.Compile(userControlSyntax, usings);
            }

            return null;
        }

    }
}
