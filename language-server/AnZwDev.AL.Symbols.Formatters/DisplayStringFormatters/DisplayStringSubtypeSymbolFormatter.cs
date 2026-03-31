using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.AL.Symbols.Formatters.FullNameFormatters
{
    internal class DisplayStringSubtypeSymbolFormatter
    {

        public void Write(TextWriter writer, SubtypeSymbol element, bool temporary)
        {
            if (!element.IsEmpty())
            {
                writer.Write(" ");
                ALLiteralFormatter.WriteName(writer, element.Name);
                if (temporary)
                    writer.Write(" temporary");
            }
        }

        public string Get(SubtypeSymbol element, bool temporary)
        {
            using (StringWriter writer = new StringWriter())
            {
                Write(writer, element, temporary);
                return writer.ToString();
            }
        }


    }
}
