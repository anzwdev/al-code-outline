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
    internal static class XmlPortFieldNodeSymbolCompiler
    {

        public static XmlPortFieldNodeSymbol? Compile(XmlPortFieldNodeSyntax syntax, XmlPortNodeKind kind, HashSet<string>? usings)
        {
            return new XmlPortFieldNodeSymbol()
            {
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Kind = kind,
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                Expression = syntax.SourceField?.ToString(),
                Schema = XmlPortNodeSymbolCompiler.Compile(syntax.Schema, usings)
            };
        }

    }
}
