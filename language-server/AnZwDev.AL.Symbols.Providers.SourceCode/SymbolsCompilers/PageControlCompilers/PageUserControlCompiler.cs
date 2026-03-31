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
    internal static class PageUserControlCompiler
    {

        public static PageControlSymbol? Compile(PageUserControlSyntax syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;

            return new PageControlSymbol()
            {
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Kind = PageControlKind.UserControl,
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                Controls = null,
                Actions = null,

                Id = 0,
                RelatedControlAddIn = ObjectReferenceCompiler.Compile(ObjectKind.ControlAddIn, usings, syntax.ControlAddIn),
                RelatedControlAddInPublicKey = null,
                TypeDefinition = null,
                RelatedPagePartId = null
            };
        }


    }
}
