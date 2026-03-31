using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    public static class SimpleTypesCompiler
    {

        public static int CompileInt(SyntaxToken token)
        {
            var textValue = token.ValueText;
            if ((!String.IsNullOrWhiteSpace(textValue)) && (Int32.TryParse(textValue, out int value)))
                return value;
            return 0;
        }

        public static int Compile(ObjectIdSyntax? syntax)
        {
            var textValue = syntax?.Value.ValueText;
            if ((!String.IsNullOrWhiteSpace(textValue)) && (Int32.TryParse(textValue, out int value)))
                return value;
            return 0;
        }

    }
}
