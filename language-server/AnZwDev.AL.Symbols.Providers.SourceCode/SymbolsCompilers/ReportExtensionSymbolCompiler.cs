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
    internal static class ReportExtensionSymbolCompiler
    {

        public static ReportExtensionSymbol Compile(ReportExtensionSyntax syntax, string? namespaceName, HashSet<string>? usings, string sourceFileName)
        {
            var id = SimpleTypesCompiler.Compile(syntax.ObjectId);
            var name = NameCompiler.Compile(syntax.Name);
            var properties = PropertySymbolCompiler.Compile(syntax.PropertyList);
            (var methods, var variables, _) = CodeMemberSymbolCompiler.Compile(syntax.Members);
            (var dataItems, var columns) = ReportExtensionDataSetSymbolCompiler.Compile(syntax.DataSet, usings);

            return new ReportExtensionSymbol(id, new FullyQualifiedName(namespaceName, name), properties)
            {
                ReferenceSourceFileName = sourceFileName,
                ExtendedObjectReference = ObjectReferenceCompiler.Compile(ObjectKind.Report, usings, syntax.BaseObject),
                Usings = usings,
                Methods = methods,
                Variables = variables,
                DataItems = dataItems,
                Columns = columns,
                Labels = ReportLabelSymbolCompiler.Compile(syntax.Labels),
                Layouts = ReportLayoutCompiler.Compile(syntax.Rendering),
                RequestPage = RequestPageExtensionSymbolCompiler.Compile(syntax.RequestPage, usings)
            };
        }

    }
}
