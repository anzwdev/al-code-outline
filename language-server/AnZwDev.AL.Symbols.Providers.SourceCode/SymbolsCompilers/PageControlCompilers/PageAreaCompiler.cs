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
    internal static class PageAreaCompiler
    {

        public static PageControlSymbol? Compile(PageAreaSyntax syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;

            return new PageControlSymbol()
            {
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Kind = PageControlKind.Area,
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                Controls = PageControlSymbolCompiler.Compile(syntax.Controls, usings),
                Actions = null,

                Id = 0,
                RelatedControlAddIn = null,
                RelatedControlAddInPublicKey = null,
                TypeDefinition = null,
                RelatedPagePartId = null
            };
        }

    }
}
