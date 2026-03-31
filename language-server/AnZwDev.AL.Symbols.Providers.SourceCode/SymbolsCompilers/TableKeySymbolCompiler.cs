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
    internal static class TableKeySymbolCompiler
    {

        public static TableKeySymbol? Compile(KeySyntax? syntax)
        {
            if (syntax == null)
                return null;

            return new TableKeySymbol()
            {
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                FieldNames = NameCompiler.Compile(syntax.Fields)
            };
        }

        public static List<TableKeySymbol> Compile(KeyListSyntax syntaxList)
        {
            List<TableKeySymbol> list = new List<TableKeySymbol>();
            if (syntaxList != null)
                Compile(syntaxList.Keys, list);
            return list;
        }

        private static void Compile(SyntaxList<KeySyntax> syntaxList, List<TableKeySymbol> list)
        {
            for (int i = 0; i < syntaxList.Count; i++)
            {
                var symbol = Compile(syntaxList[i]);
                if (symbol != null)
                    list.Add(symbol);
            }
        }


    }
}
