using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Parsing;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class PageSymbolCompiler
    {

        public static PageSymbol Compile(PageSyntax syntax, string? namespaceName, HashSet<string>? usings, string sourceFileName)
        {
            var id = SimpleTypesCompiler.Compile(syntax.ObjectId);
            var name = NameCompiler.Compile(syntax.Name);
            var properties = PropertySymbolCompiler.Compile(syntax.PropertyList);
            (var methods, var variables, _) = CodeMemberSymbolCompiler.Compile(syntax.Members);

            var actions = PageActionSymbolCompiler.Compile(syntax.Actions);
            var hasActionsV2 = (actions != null) && (actions.Any(p => (p.Kind == PageActionKind.ActionRef)));

            var sourceTableReference = ALSymbolExpressionParser.ParseObjectReference(ObjectKind.Table, properties.SourceTable, usings);

            return new PageSymbol(id, new FullyQualifiedName(namespaceName, name), properties)
            {
                ReferenceSourceFileName = sourceFileName,
                Usings = usings,
                Methods = methods,
                Variables = variables,
                Controls = PageControlSymbolCompiler.Compile(syntax.Layout, usings),
                Actions = actions,
                Views = PageViewSymbolCompiler.Compile(syntax.Views, usings),
                HasActionsV2 = hasActionsV2,
                SourceTable = sourceTableReference
            };
        }

    }
}
