using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class TableSymbolCompiler
    {

        public static TableSymbol Compile(TableSyntax syntax, string? namespaceName, HashSet<string>? usings, string sourceFileName)
        {
            var id = SimpleTypesCompiler.Compile(syntax.ObjectId);
            var name = NameCompiler.Compile(syntax.Name);
            var properties = PropertySymbolCompiler.Compile(syntax.PropertyList);
            (var methods, var variables, _) = CodeMemberSymbolCompiler.Compile(syntax.Members);

            return new TableSymbol(id, new FullyQualifiedName(namespaceName, name), properties)
            {
                ReferenceSourceFileName = sourceFileName,
                Usings = usings,                
                Methods = methods,
                Variables = variables,
                Fields = TableFieldSymbolCompiler.Compile(syntax.Fields),
                Keys = TableKeySymbolCompiler.Compile(syntax.Keys),
                FieldGroups = TableFieldGroupSymbolCompiler.Compile(syntax.FieldGroups)
            };
        }

    }
}
