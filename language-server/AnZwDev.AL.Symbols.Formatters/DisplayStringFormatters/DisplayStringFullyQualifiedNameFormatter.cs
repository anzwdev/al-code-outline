using AnZwDev.AL.Syntax;
using AnZwDev.AL.Syntax.Formatters;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Formatters.FullNameFormatters
{
    internal class DisplayStringFullyQualifiedNameFormatter : ALSyntaxElementStringFormatter<FullyQualifiedName>
    {

        public override string Get(FullyQualifiedName element)
        {
            if (!String.IsNullOrWhiteSpace(element.Namespace))
                return element.Namespace + "." + ALLiteralFormatter.GetName(element.Name);
            else
                return ALLiteralFormatter.GetName(element.Name);
        }

    }
}
