using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Parsing.Parsers
{
    internal class TableFieldExpressionReferenceParser
    {

        public string? Parse(string? expression)
        {
            if (String.IsNullOrWhiteSpace(expression))
                return null;

            //remove rec prefix
            expression = expression.Trim();

            //remove "rec." prefix
            if (expression.StartsWith(ALLanguageFacts.TableFieldExpressionPrefix, StringComparison.OrdinalIgnoreCase))
                expression = expression.Substring(ALLanguageFacts.TableFieldExpressionPrefix.Length).Trim();

            //check if expression is name without any special characters
            if (ALLiteralParser.IsValidName(expression))
                return ALLiteralParser.ParseName(expression);

            return null;
        }


    }
}
