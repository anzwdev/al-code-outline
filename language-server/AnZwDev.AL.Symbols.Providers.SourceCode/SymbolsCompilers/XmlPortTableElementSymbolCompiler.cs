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
    internal static class XmlPortTableElementSymbolCompiler
    {

        public static XmlPortTableElementSymbol? Compile(XmlPortTableElementSyntax syntax, HashSet<string>? usings)
        {
            return new XmlPortTableElementSymbol()
            {
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Kind = XmlPortNodeKind.TableElement,
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                SourceTable = ObjectReferenceCompiler.Compile(ObjectKind.Table, usings, syntax.SourceTable),
                Schema = XmlPortNodeSymbolCompiler.Compile(syntax.Schema, usings)
            };
        }


    }
}
