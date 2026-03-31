using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers;
using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.DataTypeCompilers;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.PageActionChangeCompilers
{
    internal static class PageActionAddChangeCompiler
    {

        public static PageActionChangeSymbol? Compile(ActionAddChangeSyntax syntax)
        {
            if (syntax == null)
                return null;

            return new PageActionChangeSymbol()
            {
                Anchor = NameCompiler.Compile(syntax.Anchor),
                ChangeKind = SourceCodeSymbolsCompiler.CompilePageActionChangeKind(syntax.ChangeKeyword),
                Properties = new PropertySymbolsCollection(),
                Actions = PageActionSymbolCompiler.Compile(syntax.Actions)
            };
        }


    }
}
