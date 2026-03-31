using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Converters
{
    internal static class XmlPortNodeKindConverter
    {

        public static ALSyntaxNodeKind ToALSyntaxNodeKind(this XmlPortNodeKind kind)
        {
            return kind switch
            {
                XmlPortNodeKind.FieldAttribute => ALSyntaxNodeKind.XmlPortFieldAttribute,
                XmlPortNodeKind.FieldElement => ALSyntaxNodeKind.XmlPortFieldElement,
                XmlPortNodeKind.TableElement => ALSyntaxNodeKind.XmlPortTableElement,
                XmlPortNodeKind.TextElement => ALSyntaxNodeKind.XmlPortTextElement,
                XmlPortNodeKind.TextAttribute => ALSyntaxNodeKind.XmlPortTextAttribute,
                _ => ALSyntaxNodeKind.XmlPortTextElement,
            };
        }

    }
}
