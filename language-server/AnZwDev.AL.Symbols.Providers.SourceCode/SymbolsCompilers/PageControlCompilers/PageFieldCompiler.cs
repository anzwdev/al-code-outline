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
    internal static class PageFieldCompiler
    {

        public static PageControlSymbol? Compile(PageFieldSyntax syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;

            var propertyList = PropertySymbolCompiler.Compile(syntax.PropertyList);

            //add source expression to properties - compiler is moving it there in symbol references
            var expression = syntax.Expression.ToString();
            if (!String.IsNullOrWhiteSpace(expression))
                propertyList.SetValue(PropertyKind.SourceExpression, expression);

            return new PageControlSymbol()
            {
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Kind = PageControlKind.Field,
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                Controls = null,
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
