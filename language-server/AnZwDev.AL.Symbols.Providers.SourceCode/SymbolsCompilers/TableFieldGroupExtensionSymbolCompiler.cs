using AnZwDev.AL.Symbols;
using AnZwDev.System.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class TableFieldGroupExtensionSymbolCompiler
    {

        public static List<TableFieldGroupExtensionSymbol> Compile(FieldGroupExtensionListSyntax syntaxList)
        {
            List<TableFieldGroupExtensionSymbol> list = new List<TableFieldGroupExtensionSymbol>();
            if (syntaxList != null)
                Compile(syntaxList.Changes, list);
            return list;
        }

        private static void Compile(SyntaxList<FieldGroupChangeBaseSyntax> syntaxList, List<TableFieldGroupExtensionSymbol> list)
        {
            for (int i = 0; i < syntaxList.Count; i++)
            {
                var symbol = Compile(syntaxList[i]);
                if (symbol != null)
                    list.Add(symbol);
            }
        }

        private static TableFieldGroupExtensionSymbol? Compile(FieldGroupChangeBaseSyntax syntax)
        {
            switch (syntax)
            {
                case FieldGroupAddChangeSyntax addSyntax:
                    return new TableFieldGroupExtensionSymbol()
                    {
                        Anchor = NameCompiler.Compile(addSyntax.Anchor).NotNull(),
                        FieldNames = NameCompiler.Compile(addSyntax.Fields)
                    };
            }

            return null;
        }

    }
}
