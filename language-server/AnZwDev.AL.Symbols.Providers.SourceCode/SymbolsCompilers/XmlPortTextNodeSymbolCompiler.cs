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
    internal class XmlPortTextNodeSymbolCompiler
    {
        public static XmlPortTextNodeSymbol? Compile(XmlPortTextNodeSyntax syntax, XmlPortNodeKind kind, HashSet<string>? usings)
        {
            return new XmlPortTextNodeSymbol()
            {
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Kind = kind,
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                Schema = XmlPortNodeSymbolCompiler.Compile(syntax.Schema, usings)
            };
        }


    }
}
