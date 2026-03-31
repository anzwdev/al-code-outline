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
    internal static class PageGroupCompiler
    {

        public static PageControlSymbol? Compile(PageGroupSyntax syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;

            return new PageControlSymbol()
            {
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Kind = ControlKindFromControlKeyword(syntax.ControlKeyword.Text),
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                Controls = PageControlSymbolCompiler.Compile(syntax.Controls, usings),
                Actions = PageActionSymbolCompiler.Compile(syntax.Actions),

                Id = 0,
                RelatedControlAddIn = null,
                RelatedControlAddInPublicKey = null,
                TypeDefinition = null,
                RelatedPagePartId = null
            };
        }

        private static PageControlKind ControlKindFromControlKeyword(string? keyword)
        {
            if (keyword != null)
            {
                keyword = keyword.ToLower().Trim();
                switch (keyword)
                {
                    case "repeater":
                        return PageControlKind.Repeater;
                    case "group":
                        return PageControlKind.Group;
                    case "cuegroup":
                        return PageControlKind.CueGroup;
                    case "fixed":
                        return PageControlKind.Fixed;
                }
            }

            return PageControlKind.Undefined;
        }



    }
}
