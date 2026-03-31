using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Syntax
{
    public static class ALLanguageFacts
    {

        public static string BooleanTrueLiteral { get; } = "true";
        public static string BooleanFalseLiteral { get; } = "false";

        public static char NameAppIdDelimiter { get; } = '#';

        public static char StringDelimiterChar { get; } = '\'';
        public static string StringDelimiterString { get; } = "'";
        public static string StringDelimiterEscapeString { get; } = "''";

        public static char NameDelimiterChar { get; } = '"';
        public static string NameDelimiterString { get; } = "\"";
        public static string NameDelimiterEscapeString { get; } = "\"\"";

        public static char FullyQualifiedNameSeparatorChar { get; } = '.';

        public static string LineCommentStart { get; } = "//";
        public static string MultiLineCommentStart { get; } = "/*";
        public static string MultiLineCommentEnd { get; } = "*/";

        public static char InvalidCharacterReplacementChar { get; } = '_';

        public static string TableFieldExpressionPrefix { get; } = "Rec.";

        public static bool IsValidNameMiddleCharacter(char c)
        {
            return
                ((c >= 'a') && (c <= 'z')) ||
                ((c >= 'A') && (c <= 'Z')) ||
                ((c >= '0') && (c <= '9')) ||
                (c == '_');
        }

        public static bool IsValidNameFirstCharacter(char c)
        {
            return
                ((c >= 'a') && (c <= 'z')) ||
                ((c >= 'A') && (c <= 'Z')) ||
                (c == '_');
        }

    }
}
