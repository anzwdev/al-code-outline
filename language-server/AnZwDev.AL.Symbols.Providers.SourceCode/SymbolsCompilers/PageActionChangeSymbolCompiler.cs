using AnZwDev.AL.Symbols.Providers;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.PageActionChangeCompilers;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class PageActionChangeSymbolCompiler
    {

        public static List<PageActionChangeSymbol>? Compile(PageExtensionActionListSyntax? syntax)
        {
            if (syntax == null)
                return null;
            return Compile(syntax.Changes);
        }

        public static List<PageActionChangeSymbol>? Compile<T>(SyntaxList<T> controlsList) where T : ActionChangeBaseSyntax
        {
            if (controlsList.Count == 0)
                return null;

            List<PageActionChangeSymbol> list = new List<PageActionChangeSymbol>();
            for (int i = 0; i < controlsList.Count; i++)
            {
                var control = Compile(controlsList[i]);
                if (control != null)
                    list.Add(control);
            }

            return list;
        }

        private static PageActionChangeSymbol? Compile(ActionChangeBaseSyntax syntax)
        {
            switch (syntax)
            {
                case ActionAddChangeSyntax addChangeSyntax:
                    return PageActionAddChangeCompiler.Compile(addChangeSyntax);
                case ActionModifyChangeSyntax modifyChangeSyntax:
                    return PageActionModifyChangeCompiler.Compile(modifyChangeSyntax);
                case ActionMoveChangeSyntax moveChangeSyntax:
                    return PageActionMoveChangeCompiler.Compile(moveChangeSyntax);
            }

            return null;
        }


    }
}
