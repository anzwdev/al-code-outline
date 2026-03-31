using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers;
using AnZwDev.System.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.PageControlCompilers
{
    internal static class PageSystemPartCompiler
    {

        public static PageControlSymbol? Compile(PageSystemPartSyntax syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;

            return new PageControlSymbol()
            {
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Kind = PageControlKind.SystemPart,
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                RelatedPagePartId = ObjectReferenceCompiler.Compile(ObjectKind.SystemPart, usings, syntax.SystemPartType),
                Controls = null,
                Actions = null,

                Id = 0,
                RelatedControlAddIn = null,
                RelatedControlAddInPublicKey = null,
                TypeDefinition = null
            };
        }


    }
}
