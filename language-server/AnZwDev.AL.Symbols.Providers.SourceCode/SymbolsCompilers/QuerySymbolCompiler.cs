using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class QuerySymbolCompiler
    {

        public static QuerySymbol Compile(QuerySyntax syntax, string? namespaceName, HashSet<string>? usings, string sourceFileName)
        {
            var id = SimpleTypesCompiler.Compile(syntax.ObjectId);
            var name = NameCompiler.Compile(syntax.Name);
            var properties = PropertySymbolCompiler.Compile(syntax.PropertyList);
            (var methods, var variables, _) = CodeMemberSymbolCompiler.Compile(syntax.Members);

            return new QuerySymbol(id, new FullyQualifiedName(namespaceName, name), properties)
            {
                ReferenceSourceFileName = sourceFileName,
                Usings = usings,
                Methods = methods,
                Variables = variables,
                Elements = QueryDataItemSymbolCompiler.Compile(syntax.Elements, usings)
            };
        }

    }
}
