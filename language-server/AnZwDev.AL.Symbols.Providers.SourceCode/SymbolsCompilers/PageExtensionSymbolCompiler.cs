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
    internal static class PageExtensionSymbolCompiler
    {

        public static PageExtensionSymbol Compile(PageExtensionSyntax syntax, string? namespaceName, HashSet<string>? usings, string sourceFileName)
        {
            var id = SimpleTypesCompiler.Compile(syntax.ObjectId);
            var name = NameCompiler.Compile(syntax.Name);
            var properties = PropertySymbolCompiler.Compile(syntax.PropertyList);
            (var methods, var variables, _) = CodeMemberSymbolCompiler.Compile(syntax.Members);

            return new PageExtensionSymbol(id, new FullyQualifiedName(namespaceName, name), properties)
            {
                ReferenceSourceFileName = sourceFileName,
                ExtendedObjectReference = ObjectReferenceCompiler.Compile(ObjectKind.Page, usings, syntax.BaseObject),
                Usings = usings,
                Methods = methods,
                Variables = variables,
                ControlChanges = PageControlChangesSymbolCompiler.Compile(syntax.Layout, usings),
                ActionChanges = PageActionChangeSymbolCompiler.Compile(syntax.Actions),
                ViewChanges = PageViewChangeSymbolCompiler.Compile(syntax.Views, usings)
            };
        }

    }
}
