using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal class TableFieldExtensionSymbolCompiler
    {

        public static List<TableFieldSymbol> Compile(FieldExtensionListSyntax syntaxList)
        {
            List<TableFieldSymbol> list = new List<TableFieldSymbol>();
            if (syntaxList != null)
                Compile(syntaxList.Fields, list);
            return list;
        }

        private static void Compile(SyntaxList<FieldBaseSyntax> syntaxList, List<TableFieldSymbol> list)
        {
            for (int i = 0; i < syntaxList.Count; i++)
            {
                var symbol = Compile(syntaxList[i]);
                if (symbol != null)
                    list.Add(symbol);
            }
        }

        private static TableFieldSymbol? Compile(FieldBaseSyntax? syntax)
        {
            switch (syntax)
            {
                case FieldSyntax fieldSyntax:
                    return TableFieldSymbolCompiler.Compile(fieldSyntax);
            }
            return null;
        }

    }
}
