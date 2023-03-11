using AnZwDev.ALTools.Core;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Extensions
{
    public static class StringExtensions
    {

        public static string NotNull(this string value)
        {
            if (value == null)
                return "";
            return value;
        }

        public static string Merge(params string[] values)
        {
            string mergedText = "";
            bool textIsEmpty = true;
            for (int i = 0; i < values.Length; i++)
            {
                if (!String.IsNullOrWhiteSpace(values[i]))
                {
                    if (textIsEmpty)
                    {
                        mergedText = values[i];
                        textIsEmpty = false;
                    }
                    else
                        mergedText = mergedText + " " + values[i];
                }
            }
            return mergedText;
        }

        public static bool EqualsOrEmpty(this string value, string value2)
        {
            return ((String.IsNullOrWhiteSpace(value)) || (value.Equals(value2, StringComparison.CurrentCultureIgnoreCase)));
        }

        public static bool EqualsToOneOf(this string value, params string[] compValues)
        {
            for (int i = 0; i < compValues.Length; i++)
            {
                if (value.Equals(compValues[i]))
                    return true;
            }
            return false;
        }

        public static int IndexOfFirst(this string text, int startIndex, params string[] values)
        {
            int pos = -1;
            for (int i = 0; i < values.Length; i++)
            {
                int partPos = text.IndexOf(values[i], startIndex);
                if ((partPos >= 0) && ((pos < 0) || (pos > partPos)))
                    pos = partPos;
            }
            return pos;
        }

        public static string RemovePrefixSuffix(this string text, List<string> prefixes, List<string> suffixes, List<string> affixes, List<string> additionalAffixesPatterns)
        {
            if (text != null)
            {
                bool found = false;
                //remove first suffix
                text = text.RemoveSuffix(suffixes, out found);
                if (found)
                    return text;

                //remove first prefix
                text = text.RemovePrefix(prefixes, out found);
                if (found)
                    return text;

                //remove first affix
                text = text.RemoveAffix(affixes, out found);
                if (found)
                    return text;

                //check additional affixes
                text = text.RemoveAffixPattern(additionalAffixesPatterns, out found);
                if (found)
                    return text;

            }
            return text;
        }

        public static string RemovePrefix(this string text, List<string> prefixes, out bool found)
        {
            found = false;
            if (prefixes != null)
            {
                for (int i = 0; i < prefixes.Count; i++)
                {
                    if ((!String.IsNullOrWhiteSpace(prefixes[i])) && (text.StartsWith(prefixes[i], StringComparison.CurrentCultureIgnoreCase)))
                    {
                        found = true;
                        return text.Substring(prefixes[i].Length).Trim();
                    }
                }
            }
            return text;
        }

        public static string RemoveSuffix(this string text, List<string> suffixes, out bool found)
        {
            found = false;
            if (suffixes != null)
            {
                for (int i = 0; i < suffixes.Count; i++)
                {
                    if ((!String.IsNullOrWhiteSpace(suffixes[i])) && (text.EndsWith(suffixes[i], StringComparison.CurrentCultureIgnoreCase)))
                    {
                        found = true;
                        return text.Substring(0, text.Length - suffixes[i].Length).Trim();
                    }
                }
            }
            return text;
        }

        public static string RemoveAffix(this string text, List<string> affixes, out bool found)
        {
            found = false;
            if (affixes != null)
            {
                text = text.RemoveSuffix(affixes, out found);
                if (found)
                    return text;
                text = text.RemovePrefix(affixes, out found);
                if (found)
                    return text;
            }
            return text;
        }


        public static string RemovePrefixPattern(this string text, List<string> prefixesPatterns, out bool found)
        {
            found = false;
            if (prefixesPatterns != null)
            {
                for (int i = 0; i < prefixesPatterns.Count; i++)
                {
                    if ((!String.IsNullOrWhiteSpace(prefixesPatterns[i])) && (text.StartsWithPatternIgnoreCase(prefixesPatterns[i])))
                    {
                        found = true;
                        return text.Substring(prefixesPatterns[i].Length).Trim();
                    }
                }
            }
            return text;
        }

        public static string RemoveSuffixPattern(this string text, List<string> suffixesPatterns, out bool found)
        {
            found = false;
            if (suffixesPatterns != null)
            {
                for (int i = 0; i < suffixesPatterns.Count; i++)
                {
                    if ((!String.IsNullOrWhiteSpace(suffixesPatterns[i])) && (text.EndsWithPatternIgnoreCase(suffixesPatterns[i])))
                    {
                        found = true;
                        return text.Substring(0, text.Length - suffixesPatterns[i].Length).Trim();
                    }
                }
            }
            return text;
        }


        public static string RemoveAffixPattern(this string text, List<string> affixesPatterns, out bool found)
        {
            found = false;
            if (affixesPatterns != null)
            {
                text = text.RemoveSuffixPattern(affixesPatterns, out found);
                if (found)
                    return text;
                text = text.RemovePrefixPattern(affixesPatterns, out found);
                if (found)
                    return text;
            }
            return text;
        }

        public static bool StartsWithPatternIgnoreCase(this string text, string pattern)
        {
            if ((text == null) || (pattern == null))
                return (text == pattern);

            if (text.Length < pattern.Length)
                return false;
            
            for (int i = 0; i < pattern.Length; i++)
                if (!text[i].IsPatternEqualIgnoreCase(pattern[i]))
                    return false;

            return true;
        }

        public static bool EndsWithPatternIgnoreCase(this string text, string pattern)
        {
            if ((text == null) || (pattern == null))
                return (text == pattern);

            if (text.Length < pattern.Length)
                return false;

            int textStartPos = text.Length - pattern.Length;
            for (int i = 0; i < pattern.Length; i++)
                if (!text[textStartPos + i].IsPatternEqualIgnoreCase(pattern[i]))
                    return false;

            return true;
        }


        public static List<string> ToSingleElementList(this string text)
        {
            List<string> list = new List<string>();
            list.Add(text);
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

        public static string RemoveDuplicateEmptyLines(this string text)
        {
            if (String.IsNullOrEmpty(text))
                return text;

            StringBuilder stringBuilder = new StringBuilder();
            StringLineReader stringReader = new StringLineReader(text);
            string prevEmptyLine = null;
            string prevContentLine = null;
            bool hasContent = false;
            string[] noEmptyLineBeforePart = { "}", "end;", "end.", "until ", "until\t", "until(" };
            string[] noEmptyLineAfter = { "{" };
            string[] noEmptyLinesBeforeFullText = { "end", "until" };

            while (stringReader.TryReadLineAndTrimEnd(out string line, out string newLineSeparator))
            {
                if (line == null)
                {
                    if (hasContent)
                        prevEmptyLine = newLineSeparator;
                }
                else
                {
                    if (prevEmptyLine != null)
                    {
                        string trimmedLine = line.TrimStart();
                        if (
                            (!trimmedLine.EqualsAny(noEmptyLinesBeforeFullText)) && 
                            (!trimmedLine.StartsWithAny(noEmptyLineBeforePart)) &&
                            ((prevContentLine == null) || (!prevContentLine.EndsWithAny(noEmptyLineAfter))))
                            stringBuilder.Append(prevEmptyLine);
                        prevEmptyLine = null;
                    }
                    stringBuilder.Append(line);
                    if (newLineSeparator != null)
                        stringBuilder.Append(newLineSeparator);

                    hasContent = true;
                    prevContentLine = line;
                }
            }

            return stringBuilder.ToString();
        }

        public static bool StartsWithAny(this string text, string[] startParts)
        {
            for (int i = 0; i < startParts.Length; i++)
                if (text.StartsWith(startParts[i], StringComparison.CurrentCultureIgnoreCase))
                    return true;
            return false;
        }

        public static bool EndsWithAny(this string text, string[] endsParts)
        {
            for (int i = 0; i < endsParts.Length; i++)
                if (text.EndsWith(endsParts[i], StringComparison.CurrentCultureIgnoreCase))
                    return true;
            return false;
        }

        public static bool EqualsAny(this string text, string[] values)
        {
            for (int i = 0; i < values.Length; i++)
                if (text.Equals(values[i], StringComparison.CurrentCultureIgnoreCase))
                    return true;
            return false;
        }

        public static bool ToBool(this string value)
        {
            return ((value != null) && (value.Equals("true", StringComparison.CurrentCultureIgnoreCase)));
        }

        public static T ToEnum<T>(this string value) where T : struct
        {
            if (Enum.TryParse<T>(value, true, out T result))
                return result;
            return default(T);
        }

    }
}
