using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class SubtypeSymbolCompiler
    {

        public static SubtypeSymbol? Compile(ObjectNameOrIdSyntax? syntax)
        {
            if (syntax == null)
                return null;

            var name = syntax.Identifier?.ToString();
            if (String.IsNullOrEmpty(name))
                return null;

            return new SubtypeSymbol()
            {
                Id = 0,
                Name = name,
                ModuleId = null
            };

        }

        public static SubtypeSymbol? Compile(ObjectNameReferenceSyntax? syntax)
        {
            if (syntax == null)
                return null;

            var name = syntax.Identifier?.ToString();
            if (String.IsNullOrEmpty(name))
                return null;

            return new SubtypeSymbol()
            {
                Id = 0,
                Name = name,
                ModuleId = null
            };

        }


    }
}
