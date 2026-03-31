using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Syntax.Parsers
{
    internal class ALStringParser
    {

        public string Parse(string? code)
        {
            if (code == null)
                return String.Empty;

            code = code.Trim();
            if (code.StartsWith(ALLanguageFacts.StringDelimiterString))
            {
                if ((code.Length > 1) && (code.EndsWith(ALLanguageFacts.StringDelimiterString)))
                    code = code.Substring(1, code.Length - 2);
                else
                    code = code.Substring(0, code.Length - 1);
                code = code.Replace(ALLanguageFacts.StringDelimiterEscapeString, ALLanguageFacts.StringDelimiterString);
            }
           
            return code;
        }

    }
}
