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
    internal static class TableFieldGroupSymbolCompiler
    {

        public static TableFieldGroupSymbol? Compile(FieldGroupSyntax? syntax)
        {
            if (syntax == null)
                return null;

            return new TableFieldGroupSymbol()
            {
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                FieldNames = NameCompiler.Compile(syntax.Fields)
            };
        }

        public static List<TableFieldGroupSymbol> Compile(FieldGroupListSyntax syntaxList)
        {
            List<TableFieldGroupSymbol> list = new List<TableFieldGroupSymbol>();
            if (syntaxList != null)
                Compile(syntaxList.FieldGroups, list);
            return list;
        }

        private static void Compile(SyntaxList<FieldGroupSyntax> syntaxList, List<TableFieldGroupSymbol> list)
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
