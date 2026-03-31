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

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.PageControlChangeCompilers
{
    internal static class PageControlMoveChangeCompiler
    {

        public static PageControlChangeSymbol? Compile(ControlMoveChangeSyntax syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;

            return new PageControlChangeSymbol()
            {
                Anchor = NameCompiler.Compile(syntax.Anchor),
                ChangeKind = SourceCodeSymbolsCompiler.CompilePageControlChangeKind(syntax.ChangeKeyword),
                Properties = new PropertySymbolsCollection(),
                Controls = PageControlSymbolCompiler.Compile(syntax.Controls)
            };
        }


    }
}
