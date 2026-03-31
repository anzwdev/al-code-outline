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
    internal static class TableFieldSymbolCompiler
    {

        public static TableFieldSymbol? Compile(FieldSyntax? syntax)
        {
            if (syntax == null)
                return null;

            return new TableFieldSymbol()
            {
                Id = SimpleTypesCompiler.CompileInt(syntax.No),
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                TypeDefinition = TypeDefinitionSymbolCompiler.Compile(syntax.Type)
            };
        }

        public static List<TableFieldSymbol> Compile(FieldListSyntax syntaxList)
        {
            List<TableFieldSymbol> list = new List<TableFieldSymbol>();
            if (syntaxList != null)
                Compile(syntaxList.Fields, list);
            return list;
        }

        private static void Compile(SyntaxList<FieldSyntax> syntaxList, List<TableFieldSymbol> list)
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
