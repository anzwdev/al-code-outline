using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System.Extensions
{
    public static class StringPrefixSuffixExtensions
    {

        public static string? RemovePrefixSuffix(this string? text, List<string>? prefixes, List<string>? suffixes, List<string>? affixes, List<string>? additionalAffixesPatterns)
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

        public static string RemovePrefix(this string text, List<string>? prefixes, out bool found)
        {
            found = false;
            if (prefixes != null)
            {
                for (int i = 0; i < prefixes.Count; i++)
                {
                    if ((!String.IsNullOrWhiteSpace(prefixes[i])) && (text.StartsWith(prefixes[i], StringComparison.OrdinalIgnoreCase)))
                    {
                        found = true;
                        return text.Substring(prefixes[i].Length).Trim();
                    }
                }
            }
            return text;
        }

        public static string RemoveSuffix(this string text, List<string>? suffixes, out bool found)
        {
            found = false;
            if (suffixes != null)
            {
                for (int i = 0; i < suffixes.Count; i++)
                {
                    if ((!String.IsNullOrWhiteSpace(suffixes[i])) && (text.EndsWith(suffixes[i], StringComparison.OrdinalIgnoreCase)))
                    {
                        found = true;
                        return text.Substring(0, text.Length - suffixes[i].Length).Trim();
                    }
                }
            }
            return text;
        }

        public static string RemoveAffix(this string text, List<string>? affixes, out bool found)
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


        public static string RemovePrefixPattern(this string text, List<string>? prefixesPatterns, out bool found)
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

        public static string RemoveSuffixPattern(this string text, List<string>? suffixesPatterns, out bool found)
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


        public static string RemoveAffixPattern(this string text, List<string>? affixesPatterns, out bool found)
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

        public static bool StartsWithPatternIgnoreCase(this string? text, string? pattern)
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

        public static bool EndsWithPatternIgnoreCase(this string? text, string? pattern)
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


    }
}
