using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Syntax.Formatters
{
    internal class ALStringFormatter : ALSyntaxElementStringFormatter<string>
    {

        public override string Get(string value)
        {
            return
                ALLanguageFacts.StringDelimiterString +
                value.Replace(ALLanguageFacts.StringDelimiterString, ALLanguageFacts.StringDelimiterEscapeString) +
                ALLanguageFacts.StringDelimiterString;
        }

    }
}
