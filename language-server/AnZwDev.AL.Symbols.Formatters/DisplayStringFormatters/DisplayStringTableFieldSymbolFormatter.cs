using AnZwDev.AL.Syntax;
using AnZwDev.AL.Syntax.Formatters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.AL.Symbols.Formatters.DisplayStringFormatters
{
    internal class DisplayStringTableFieldSymbolFormatter : ALSyntaxElementWriterFormatter<TableFieldSymbol>
    {

        public override void Write(TextWriter writer, TableFieldSymbol element)
        {
            ALLiteralFormatter.WriteName(writer, element.Name);
            if ((element.TypeDefinition != null) && (!element.TypeDefinition.IsEmpty()))
            {
                writer.Write(": ");
                DisplayStringFormatter.WriteTypeDefinitionSymbol(writer, element.TypeDefinition);
            }

            var hasAttributes = false;

            var fieldClass = (element.Properties != null) ? element.Properties.FieldClass : FieldClass.Normal;
            hasAttributes = WriteAttribute(writer, fieldClass.ToString(), hasAttributes);

            //add properties
            if (element.Properties != null)
            {

                if (!element.Properties.Enabled)
                    hasAttributes = WriteAttribute(writer, "Disabled", hasAttributes);

                var obsoleteState = element.Properties.ObsoleteState;
                if (obsoleteState != ObsoleteState.No)
                    hasAttributes = WriteAttribute(writer, obsoleteState.ToString(), hasAttributes);
            }

            if (hasAttributes)
                writer.Write(")");
        }

        private bool WriteAttribute(TextWriter writer, string attribute, bool hasAttributes)
        {
            if (String.IsNullOrWhiteSpace(attribute))
                return hasAttributes;

            if (hasAttributes)
                writer.Write(", ");
            else
                writer.Write(" (");

            writer.Write(attribute);
            return true;
        }

    }
}
