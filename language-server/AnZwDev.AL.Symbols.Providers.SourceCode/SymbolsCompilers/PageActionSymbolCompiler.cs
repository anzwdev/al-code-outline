using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.PageActionCompilers;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class PageActionSymbolCompiler
    {

        public static List<PageActionSymbol>? Compile(PageActionListSyntax? syntax)
        {
            if (syntax == null)
                return null;
            return Compile(syntax.ActionList);
        }

        public static List<PageActionSymbol>? Compile<T>(SyntaxList<T> actionsList) where T : ActionBaseSyntax
        {
            if (actionsList.Count == 0)
                return null;

            List<PageActionSymbol> list = new List<PageActionSymbol>();
            for (int i = 0; i < actionsList.Count; i++)
            {
                var action = Compile(actionsList[i]);
                if (action != null)
                    list.Add(action);
            }

            return list;
        }

        public static List<PageActionSymbol> Compile(SeparatedSyntaxList<IdentifierNameSyntax> syntax)
        {
            if (syntax.Count == 0)
                return new List<PageActionSymbol>();

            var list = new List<PageActionSymbol>(syntax.Count);
            for (int i = 0; i < syntax.Count; i++)
            {
                var item = Compile(syntax[i]);
                if (item != null)
                    list.Add(item);
            }
            return list;
        }


        private static PageActionSymbol? Compile(IdentifierNameSyntax? syntax)
        {
            var name = NameCompiler.Compile(syntax);
            if (name == null)
                return null;

            return new PageActionSymbol()
            {
                Name = name,
                Kind = PageActionKind.Undefined,
                Actions = null,
                Properties = null,
                Id = 0,
                TargetId = 0,
                TargetName = null
            };
        }

        private static PageActionSymbol? Compile(ActionBaseSyntax syntax)
        {
            switch (syntax)
            {
                case PageActionAreaSyntax areaSyntax:
                    return PageActionAreaCompiler.Compile(areaSyntax);
                case PageActionGroupSyntax groupSyntax:
                    return PageActionGroupCompiler.Compile(groupSyntax);
                case PageActionSyntax actionSyntax:
                    return PageActionCompiler.Compile(actionSyntax);
                case PageActionSeparatorSyntax separatorSyntax:
                    return PageActionSeparatorCompiler.Compile(separatorSyntax);
                case PageActionRefSyntax refSyntax:
                    return PageActionRefCompiler.Compile(refSyntax);
                case PageCustomActionSyntax customActionSyntax:
                    return PageCustomActionCompiler.Compile(customActionSyntax);
                case PageSystemActionSyntax systemActionSyntax:
                    return PageSystemActionCompiler.Compile(systemActionSyntax);
                case PageFileUploadActionSyntax pageFileUploadActionSyntax:
                    return PageFileUploadActionCompiler.Compile(pageFileUploadActionSyntax);
            }

            return null;
        }

    }
}
