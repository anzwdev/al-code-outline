using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class PageCustomizationSymbolCompiler
    {

        public static PageCustomizationSymbol Compile(PageCustomizationSyntax syntax, string? namespaceName, HashSet<string>? usings, string sourceFileName)
        {
            var id = SimpleTypesCompiler.Compile(syntax.ObjectId);
            var name = NameCompiler.Compile(syntax.Name);
            var properties = PropertySymbolCompiler.Compile(syntax.PropertyList);

            return new PageCustomizationSymbol(id, new FullyQualifiedName(namespaceName, name), properties)
            {
                ReferenceSourceFileName = sourceFileName,
                ExtendedObjectReference = ObjectReferenceCompiler.Compile(ObjectKind.Page, usings, syntax.BaseObject),
                Usings = usings,
                ControlChanges = PageControlChangesSymbolCompiler.Compile(syntax.Layout, usings)                
            };
        }


    }
}
