using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class ReportSymbolCompiler
    {

        public static ReportSymbol Compile(ReportSyntax syntax, string? namespaceName, HashSet<string>? usings, string sourceFileName)
        {
            var id = SimpleTypesCompiler.Compile(syntax.ObjectId);
            var name = NameCompiler.Compile(syntax.Name);
            var properties = PropertySymbolCompiler.Compile(syntax.PropertyList);
            (var methods, var variables, _) = CodeMemberSymbolCompiler.Compile(syntax.Members);

            return new ReportSymbol(id, new FullyQualifiedName(namespaceName, name), properties)
            {
                ReferenceSourceFileName = sourceFileName,
                Usings = usings,
                Methods = methods,
                Variables = variables,
                DataItems = ReportDataItemSymbolCompiler.Compile(syntax.DataSet, usings),
                Labels = ReportLabelSymbolCompiler.Compile(syntax.Labels),
                Layouts = ReportLayoutCompiler.Compile(syntax.Rendering),
                RequestPage = RequestPageSymbolCompiler.Compile(syntax.RequestPage, usings)
            };
        }

    }
}
