using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.PageControlChangeCompilers;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class PageControlChangesSymbolCompiler
    {

        public static List<PageControlChangeSymbol>? Compile(PageExtensionLayoutSyntax? syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;
            return Compile(syntax.Changes, usings);
        }

        public static List<PageControlChangeSymbol>? Compile<T>(SyntaxList<T> controlsList, HashSet<string>? usings) where T : ControlChangeBaseSyntax
        {
            if (controlsList.Count == 0)
                return null;

            List<PageControlChangeSymbol> list = new List<PageControlChangeSymbol>();
            for (int i = 0; i < controlsList.Count; i++)
            {
                var control = Compile(controlsList[i], usings);
                if (control != null)
                    list.Add(control);
            }

            return list;
        }

        private static PageControlChangeSymbol? Compile(ControlChangeBaseSyntax syntax, HashSet<string>? usings)
        {
            switch (syntax)
            {
                case ControlAddChangeSyntax addChangeSyntax:
                    return PageControlAddChangeCompiler.Compile(addChangeSyntax, usings);
                case ControlModifyChangeSyntax modifyChangeSyntax:
                    return PageControlModifyChangeCompiler.Compile(modifyChangeSyntax, usings);
                case ControlMoveChangeSyntax controlMoveChangeSyntax:
                    return PageControlMoveChangeCompiler.Compile(controlMoveChangeSyntax, usings);
            }

            return null;
        }

    }
}
