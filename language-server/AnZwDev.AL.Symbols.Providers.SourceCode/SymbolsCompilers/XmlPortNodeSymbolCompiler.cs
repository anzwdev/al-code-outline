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
    internal static class XmlPortNodeSymbolCompiler
    {

        public static List<XmlPortNodeSymbol>? Compile(XmlPortSchemaSyntax? syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;

            return Compile(syntax.XmlPortSchema, usings);
        }


        public static List<XmlPortNodeSymbol>? Compile(SyntaxList<XmlPortNodeSyntax> itemsList, HashSet<string>? usings)
        {
            if (itemsList.Count == 0)
                return null;

            List<XmlPortNodeSymbol> list = new List<XmlPortNodeSymbol>();
            for (int i = 0; i < itemsList.Count; i++)
            {
                var node = Compile(itemsList[i], usings);
                if (node != null)
                    list.Add(node);
            }

            return list;
        }

        public static XmlPortNodeSymbol? Compile(XmlPortNodeSyntax syntax, HashSet<string>? usings)
        {
            switch (syntax)
            {
                case XmlPortTableElementSyntax tableElementSyntax:
                    return XmlPortTableElementSymbolCompiler.Compile(tableElementSyntax, usings);
                case XmlPortFieldElementSyntax fieldElementSyntax:
                    return XmlPortFieldNodeSymbolCompiler.Compile(fieldElementSyntax, XmlPortNodeKind.FieldElement, usings);
                case XmlPortFieldAttributeSyntax fieldAttributeSyntax:
                    return XmlPortFieldNodeSymbolCompiler.Compile(fieldAttributeSyntax, XmlPortNodeKind.FieldAttribute, usings);
                case XmlPortTextElementSyntax textElementSyntax:
                    return XmlPortTextNodeSymbolCompiler.Compile(textElementSyntax, XmlPortNodeKind.TextElement, usings);
                case XmlPortTextAttributeSyntax textAttributeSyntax:
                    return XmlPortTextNodeSymbolCompiler.Compile(textAttributeSyntax, XmlPortNodeKind.TextAttribute, usings);
                default:
                    return null;
            }
        }

    }
}
