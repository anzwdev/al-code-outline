using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class RequestPageExtensionSymbolCompiler
    {

        public static RequestPageExtensionSymbol? Compile(RequestPageExtensionSyntax? syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;

            (var methods, var variables, _) = CodeMemberSymbolCompiler.Compile(syntax.Members);

            return new RequestPageExtensionSymbol()
            {
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                Methods = methods,
                Variables = variables,
                ControlChanges = PageControlChangesSymbolCompiler.Compile(syntax.Layout, usings),
                ActionChanges = PageActionChangeSymbolCompiler.Compile(syntax.Actions),
            };
        }

    }
}
