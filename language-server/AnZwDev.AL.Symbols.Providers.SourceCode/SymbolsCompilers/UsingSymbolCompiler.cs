using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    public static class UsingSymbolCompiler
    {

        public static string? Compile(UsingDirectiveSyntax? syntax)
        {
            return NameCompiler.Compile(syntax?.Name);
        }

        public static HashSet<string>? Compile(SyntaxList<UsingDirectiveSyntax> syntaxList)
        {
            HashSet<string>? usings = null;
            if (syntaxList.Count > 0)
                for (int i = 0; i < syntaxList.Count; i++)
                {
                    var usingValue = UsingSymbolCompiler.Compile(syntaxList[i]);
                    if (!String.IsNullOrEmpty(usingValue))
                    {
                        if (usings == null)
                            usings = new HashSet<string>();
                        usings.Add(usingValue);
                    }
                }
            return usings;
        }


    }
}
