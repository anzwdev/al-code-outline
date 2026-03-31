using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class AttributeParameterSymbolCompiler
    {

        public static string Compile(AttributeArgumentSyntax? syntax)
        {
            if (syntax == null)
                return String.Empty;
            return syntax.ToString();
        }

        public static List<string>? Compile(AttributeArgumentListSyntax? syntax)
        {
            if (syntax == null || syntax.Arguments.Count == 0)
                return null;

            var list = new List<string>();
            for (int i = 0; i < syntax.Arguments.Count; i++)
                list.Add(Compile(syntax.Arguments[i]));
            return list;
        }


    }
}
