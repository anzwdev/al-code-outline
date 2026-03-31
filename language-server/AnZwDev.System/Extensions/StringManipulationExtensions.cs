using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System.Extensions
{
    public static class StringManipulationExtensions
    {

        public static string NotNull(this string? value)
        {
            return value ?? String.Empty;
        }

        public static (string?, string) TryRemoveFirstPart(this string value, string partStartIdentifier, string partEndIdentifier, bool includeStartEnd)
        {
            if (value.StartsWith(partStartIdentifier))
            {
                var partStartIdentifierLength = partStartIdentifier.Length;
                var partEndPos = value.IndexOf(partEndIdentifier, partStartIdentifierLength);
                if (partEndPos >= 0)
                {
                    if (includeStartEnd)
                    {
                        partEndPos += partEndIdentifier.Length;
                        return (value.Substring(0, partEndPos), value.Substring(partEndPos));
                    }
                    
                    return (
                        value.Substring(partStartIdentifierLength, partEndPos - partStartIdentifierLength),
                        value.Substring(partEndPos + partEndIdentifier.Length));
                }
            }

            return (null, value);
        }

        public static string FirstLine(this string value)
        {
            var pos = value.IndexOf('\n');
            if (pos >= 0)
                return value.Substring(0, pos);
            return value;
        }

        public static string LimitLength(this string value, int maxLength)
        {
            if (value.Length > maxLength)
                return value.Substring(0, maxLength);
            return value;
        }

        public static string FirstWord(this string text)
        {
            for (int i = 0; i < text.Length; i++)
                if (!text[i].IsValidWordCharacter())
                    return text.Substring(0, i);
            return text;
        }

        public static List<string>? SplitWithDelimiters(this string? value, char delimiter, char separator, bool trim = true, bool includeEmptyValues = false)
        {
            if (value == null)
                return null;

            var list = new List<string>();

            int startPos = 0;
            int pos = 0;
            bool insideDelimiter = false;
            while (pos < value.Length)
            {
                var ch = value[pos];

                if (ch == delimiter)
                    insideDelimiter = !insideDelimiter;
                else if ((!insideDelimiter) && (ch == separator))
                {
                    var part = value.Substring(startPos, pos - startPos);
                    if (trim)
                        part = part.Trim();
                    if (includeEmptyValues || part.Length > 0)
                        list.Add(part);
                    startPos = pos + 1;
                }
                pos++;
            }

            if (startPos < value.Length)
            {
                var part = value.Substring(startPos);
                if (trim)
                    part = part.Trim();
                if (includeEmptyValues || part.Length > 0)
                    list.Add(part);
            }

            return list;
        }

        public static string MultilineTrimEnd(this string text)
        {
            if (String.IsNullOrEmpty(text))
                return text;

            StringBuilder stringBuilder = new StringBuilder();
            int startPos = 0;
            int endPos = -1;

            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];
                bool isNewLine = (character == '\n') || (character == '\r');
                bool isWhitespace = (Char.IsWhiteSpace(character) && (!isNewLine));

                if (!isWhitespace)
                {
                    if (isNewLine)
                    {
                        if (endPos >= startPos)
                            stringBuilder.Append(text.Substring(startPos, endPos + 1 - startPos));
                        startPos = i;
                    }
                    endPos = i;
                }
            }
            if (endPos >= startPos)
                stringBuilder.Append(text.Substring(startPos, endPos + 1 - startPos));

            return stringBuilder.ToString();
        }

    }
}
