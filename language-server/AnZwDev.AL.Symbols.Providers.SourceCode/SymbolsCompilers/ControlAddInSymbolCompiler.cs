using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class ControlAddInSymbolCompiler
    {

        public static ControlAddInSymbol Compile(ControlAddInSyntax syntax, string? namespaceName, HashSet<string>? usings, string sourceFileName)
        {
            var id = 0;
            var name = NameCompiler.Compile(syntax.Name);
            var properties = PropertySymbolCompiler.Compile(syntax.PropertyList);
            (var methods, var variables, var events) = CodeMemberSymbolCompiler.Compile(syntax.Members);

            return new ControlAddInSymbol(id, new FullyQualifiedName(namespaceName, name), properties)
            {
                ReferenceSourceFileName = sourceFileName,
                Usings = usings,
                Methods = methods,
                Events = events,
                MetadataName = null,
                PublicKeyToken = null
            };
        }

    }
}
