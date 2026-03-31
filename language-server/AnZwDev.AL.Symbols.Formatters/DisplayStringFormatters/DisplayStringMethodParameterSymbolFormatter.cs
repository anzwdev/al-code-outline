using AnZwDev.AL.Syntax.Formatters;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Formatters.FullNameFormatters
{
    internal class DisplayStringMethodParameterSymbolFormatter : DisplayStringVariableDeclarationSymbolFormatter<MethodParameterSymbol>
    {

        public override void Write(TextWriter writer, MethodParameterSymbol element)
        {
            if (element.IsVar)
                writer.Write("var ");
            base.Write(writer, element);
        }

    }
}
