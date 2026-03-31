using AnZwDev.AL.Syntax;
using AnZwDev.AL.Syntax.Formatters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.AL.Symbols.Formatters.FullNameFormatters
{
    internal class DisplayStringTypeDefinitionSymbolFormatter : ALSyntaxElementWriterFormatter<TypeDefinitionSymbol>
    {

        public override void Write(TextWriter writer, TypeDefinitionSymbol element)
        {
            //append array
            if ((element.ArrayDimensions != null) && (element.ArrayDimensions.Count > 0))
            {
                writer.Write("array[");
                for (int i = 0; i < element.ArrayDimensions.Count; i++)
                {
                    if (i > 0)
                        writer.Write(",");
                    ALLiteralFormatter.WriteInt(writer, element.ArrayDimensions[i]);
                }
                writer.Write("] of ");
            }

            //type name
            ALLiteralFormatter.WriteName(writer, element.Name);
            if (element.Subtype != null)
                DisplayStringFormatter.WriteSubtypeSymbol(writer, element.Subtype, element.Temporary);

            //type arguments
            if ((element.TypeArguments != null) && (element.TypeArguments.Count > 0))
            {
                writer.Write(" of [");
                for (int i = 0; i < element.TypeArguments.Count; i++)
                {
                    if (i > 0)
                        writer.Write(", ");
                    DisplayStringFormatter.WriteTypeDefinitionSymbol(writer, element.TypeArguments[i]);
                }
                writer.Write("]");
            }

        }

    }
}
