using AnZwDev.AL.Syntax;
using AnZwDev.AL.Syntax.Formatters;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.AL.Symbols.Formatters.FullNameFormatters
{
    internal class DisplayStringMethodSymbolFormatter : ALSyntaxElementWriterFormatter<MethodSymbol>
    {

        public override void Write(TextWriter writer, MethodSymbol element)
        {
            ALLiteralFormatter.WriteName(writer, element.Name);
            writer.Write("(");
            if (element.Parameters != null)
            {
                for (int i = 0; i < element.Parameters.Count; i++)
                {
                    if (i > 0)
                        writer.Write("; ");
                    DisplayStringFormatter.WriteParameterSymbol(writer, element.Parameters[i]);
                }
            }
            writer.Write(")");

            if (element.ReturnParameterDefinition != null)
                DisplayStringFormatter.WriteReturnParameter(writer, element.ReturnParameterDefinition);
        }

    }
}
