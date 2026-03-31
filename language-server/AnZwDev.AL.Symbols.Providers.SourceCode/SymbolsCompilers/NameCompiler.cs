using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.AL.Syntax;
using AnZwDev.System.Extensions;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    public static class NameCompiler
    {

        public static string Compile(IdentifierNameOrEmptySyntax? syntax)
        {
            return ALLiteralParser.ParseName(syntax?.IdentifierName?.Identifier.Text);
        }

        public static string Compile(IdentifierNameSyntax? syntax)
        {
            return ALLiteralParser.ParseName(syntax?.Identifier.Text);
        }

        public static string Compile(NameSyntax? syntax)
        {
            return ALLiteralParser.ParseName(syntax?.ToString());
        }

        public static List<string> Compile(SeparatedSyntaxList<IdentifierNameSyntax> syntaxList)
        {
            var list = new List<string>(syntaxList.Count);

            for (int i = 0; i < syntaxList.Count; i++)
            {
                var name = Compile(syntaxList[i]);
                if (name != null)
                    list.Add(name);
            }

            return list;
        }

        public static List<string> Compile(SeparatedSyntaxList<IdentifierNameOrEmptySyntax> syntaxList)
        {
            var list = new List<string>(syntaxList.Count);

            for (int i = 0; i < syntaxList.Count; i++)
            {
                var name = Compile(syntaxList[i]);
                if (name != null)
                    list.Add(name);
            }

            return list;
        }


    }
}
