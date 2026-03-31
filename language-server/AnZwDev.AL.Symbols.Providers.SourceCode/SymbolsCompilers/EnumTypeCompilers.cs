using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Parsing;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class EnumTypeCompilers
    {

        public static ObjectKind CompileObjectType(SyntaxToken syntax)
        {
            var text = syntax.Text.ToLower();
            return ALSymbolExpressionParser.ParseObjectKind(text);
        }

    }
}
