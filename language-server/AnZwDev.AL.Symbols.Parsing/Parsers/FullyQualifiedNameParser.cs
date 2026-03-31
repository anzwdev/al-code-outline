using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Parsing.Parsers
{
    internal class FullyQualifiedNameParser
    {

        public FullyQualifiedName Parse(string? value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return new FullyQualifiedName() { Namespace = null, Name = String.Empty };

            var delimiterPos = FindDelimiter(value);
            if (delimiterPos < 0)
                return new FullyQualifiedName() { Namespace = null, Name = ALLiteralParser.ParseName(value) };

            return new FullyQualifiedName()
            {
                Namespace = value.Substring(0, delimiterPos).Trim(),
                Name = ALLiteralParser.ParseName(value.Substring(delimiterPos + 1))
            };
        }

        private int FindDelimiter(string code)
        {
            var delimiterPos = -1;
            var inQuotes = false;

            for (int i = 0; i < code.Length; i++)
            {
                if (code[i] == ALLanguageFacts.StringDelimiterChar)
                    inQuotes = !inQuotes;
                else if ((code[i] == ALLanguageFacts.FullyQualifiedNameSeparatorChar) && (!inQuotes))
                    delimiterPos = i;
            }
            return delimiterPos;
        }

    }
}
