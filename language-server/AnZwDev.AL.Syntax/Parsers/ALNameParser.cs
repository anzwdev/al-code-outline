using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.AL.Syntax.Parsers
{
    internal class ALNameParser
    {

        public string Parse(string? code)
        {
            if (code == null)
                return String.Empty;

            code = code.Trim();
            if (code.StartsWith(ALLanguageFacts.NameDelimiterChar))
            {
                if ((code.Length > 1) && (code.EndsWith(ALLanguageFacts.NameDelimiterChar)))
                    code = code.Substring(1, code.Length - 2);
                else
                    code = code.Substring(0, code.Length - 1);
                code = code.Replace(ALLanguageFacts.NameDelimiterEscapeString, ALLanguageFacts.NameDelimiterString);
            }
            return code;
        }

        public bool IsValid(string? code)
        {
            if (String.IsNullOrWhiteSpace(code))
                return false;
            code = code.Trim();

            bool isEscaped = false;
            bool inEscapedName = false;
            for (int i=0; i < code.Length; i++)
            {
                var currentChar = code[i];

                if (i==0)
                {
                    if (currentChar == ALLanguageFacts.NameDelimiterChar)
                    {
                        isEscaped = true;
                        inEscapedName = true;
                    }
                    else if (!ALLanguageFacts.IsValidNameFirstCharacter(currentChar))
                        return false;
                } 
                else
                {
                    if (currentChar == ALLanguageFacts.NameDelimiterChar)
                    {
                        if (!isEscaped)
                            return false;
                        inEscapedName = !inEscapedName;
                    }
                    else if ((!inEscapedName) && (!ALLanguageFacts.IsValidNameMiddleCharacter(currentChar)))
                        return false;
                }
            }

            return true;
        }

    }
}
