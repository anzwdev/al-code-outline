using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class TableExtensionSymbolCompiler
    {

        public static TableExtensionSymbol Compile(TableExtensionSyntax syntax, string? namespaceName, HashSet<string>? usings, string sourceFileName)
        {
            var id = SimpleTypesCompiler.Compile(syntax.ObjectId);
            var name = NameCompiler.Compile(syntax.Name);
            var properties = PropertySymbolCompiler.Compile(syntax.PropertyList);
            (var methods, var variables, _) = CodeMemberSymbolCompiler.Compile(syntax.Members);

            return new TableExtensionSymbol(id, new FullyQualifiedName(namespaceName, name), properties)
            {
                ReferenceSourceFileName = sourceFileName,
                Usings = usings,
                Methods = methods,
                Variables = variables,
                Fields = TableFieldExtensionSymbolCompiler.Compile(syntax.Fields),
                FieldGroups = TableFieldGroupExtensionSymbolCompiler.Compile(syntax.FieldGroups),
                Keys = TableKeySymbolCompiler.Compile(syntax.Keys),
                ExtendedObjectReference = ObjectReferenceCompiler.Compile(ObjectKind.Table, usings, syntax.BaseObject)
            };
        }


    }
}
