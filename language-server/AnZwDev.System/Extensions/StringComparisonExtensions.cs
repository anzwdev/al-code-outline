using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System.Extensions
{
    public static class StringComparisonExtensions
    {

        public static bool EqualsAny(this string value, StringComparison stringComparison, params string[] compareWith)
        {
            for (int i = 0; i < compareWith.Length; i++)
                if (String.Equals(value, compareWith[i], stringComparison))
                    return true;
            return false;
        }

        public static bool EqualsAny(this string value, params string[] compareWith)
        {
            for (int i = 0; i < compareWith.Length; i++)
                if (String.Equals(value, compareWith[i]))
                    return true;
            return false;
        }

        public static bool StartsWithAny(this string value, StringComparison stringComparison, params string[] compareWith)
        {
            for (int i = 0; i < compareWith.Length; i++)
                if (value.StartsWith(compareWith[i], stringComparison))
                    return true;
            return false;
        }

        public static bool StartsWithAny(this string value, params string[] compareWith)
        {
            for (int i = 0; i < compareWith.Length; i++)
                if (value.StartsWith(compareWith[i]))
                    return true;
            return false;
        }

        public static bool EndsWithAny(this string value, StringComparison stringComparison, params string[] compareWith)
        {
            for (int i = 0; i < compareWith.Length; i++)
                if (value.EndsWith(compareWith[i], stringComparison))
                    return true;
            return false;
        }

        public static bool EndsWithAny(this string value, params string[] compareWith)
        {
            for (int i = 0; i < compareWith.Length; i++)
                if (value.EndsWith(compareWith[i]))
                    return true;
            return false;
        }

        public static bool EqualsOrEmpty(this string value, string compareWith)
        {
            return String.IsNullOrEmpty(value) || value.Equals(compareWith);
        }

        public static bool EqualsOrEmpty(this string value, string compareWith, StringComparison stringComparison)
        {
            return String.IsNullOrEmpty(value) || value.Equals(compareWith, stringComparison);
        }

        public static int IndexOfFirst(this string text, int startIndex, params string[] values)
        {
            int pos = -1;
            for (int i = 0; i < values.Length; i++)
            {
                int valuePos = text.IndexOf(values[i], startIndex);
                if ((valuePos >= 0) && ((pos < 0) || (pos > valuePos)))
                    pos = valuePos;
            }
            return pos;
        }

    }
}
