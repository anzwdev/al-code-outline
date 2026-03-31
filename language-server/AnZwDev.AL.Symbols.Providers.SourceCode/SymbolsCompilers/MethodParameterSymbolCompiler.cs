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
    internal static class MethodParameterSymbolCompiler
    {

        public static MethodParameterSymbol Compile(ParameterSyntax? syntax)
        {
            var varText = syntax?.VarKeyword.Text;

            return new MethodParameterSymbol()
            {
                Name = NameCompiler.Compile(syntax?.Name).NotNull(),
                TypeDefinition = TypeDefinitionSymbolCompiler.Compile(syntax?.Type),
                IsVar = (varText != null) && (varText.Equals("var", StringComparison.InvariantCultureIgnoreCase)),
                Attributes = null                
            };
        }

        public static List<MethodParameterSymbol>? Compile(ParameterListSyntax? syntax)
        {
            if ((syntax?.Parameters == null) || (syntax.Parameters.Count == 0))
                return null;

            var list = new List<MethodParameterSymbol>();
            for (int i = 0; i < syntax.Parameters.Count; i++)
                list.Add(Compile(syntax.Parameters[i]));
            return list;
        }

    }
}
