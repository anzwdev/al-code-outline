using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Parsing.Parsers
{
    internal class LabelParser 
    {

        public Label Parse(string? value, Dictionary<string, string>? valueProperties)
        {
            var locked = false;
            string? comment = null;
            
            if (valueProperties != null)
            {
                if (valueProperties.ContainsKey("Locked"))
                    locked = ALLiteralParser.ParseBool(valueProperties["Locked"]);

                if (valueProperties.ContainsKey("Comment"))
                    comment = valueProperties["Comment"];
            }

            return new Label()
            {
                Text = value,
                Locked = locked,
                Comment = comment
            };
        }

    }
}
