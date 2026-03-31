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
    internal static class AttributeSymbolCompiler
    {

        public static AttributeSymbol Compile(MemberAttributeSyntax? syntax)
        {
            var name = NameCompiler.Compile(syntax?.Name).NotNull();
            var arguments = AttributeParameterSymbolCompiler.Compile(syntax?.ArgumentList);

            return new AttributeSymbol()
            {
                Name = name,
                Arguments = arguments
            };
        }

        public static List<AttributeSymbol>? CompileList(SyntaxList<MemberAttributeSyntax> syntaxList)
        {
            if (syntaxList.Count == 0)
                return null;

            var list = new List<AttributeSymbol>();
            for (int i=0; i < syntaxList.Count; i++)
                list.Add(Compile(syntaxList[i]));
            return list;
        }

    }
}
