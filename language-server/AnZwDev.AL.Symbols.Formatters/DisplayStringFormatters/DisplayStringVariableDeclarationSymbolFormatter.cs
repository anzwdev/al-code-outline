using AnZwDev.AL.Syntax;
using AnZwDev.AL.Syntax.Formatters;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.AL.Symbols.Formatters.FullNameFormatters
{

    internal class DisplayStringVariableDeclarationSymbolFormatter : DisplayStringVariableDeclarationSymbolFormatter<VariableDeclarationSymbol>
    {
    }

    internal class DisplayStringVariableDeclarationSymbolFormatter<T> : ALSyntaxElementWriterFormatter<T> where T : VariableDeclarationSymbol
    {

        public override void Write(TextWriter writer, T element)
        {
            if (!String.IsNullOrWhiteSpace(element.Name))
                ALLiteralFormatter.WriteName(writer, element.Name);
            writer.Write(": ");
            if ((element.TypeDefinition != null) && (!element.TypeDefinition.IsEmpty()))
                DisplayStringFormatter.WriteTypeDefinitionSymbol(writer, element.TypeDefinition);
        }

    }
}
