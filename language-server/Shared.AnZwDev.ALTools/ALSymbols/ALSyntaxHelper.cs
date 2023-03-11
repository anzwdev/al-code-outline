using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALSyntaxHelper
    {

        public static bool NameNeedsEcoding(string name)
        {
            if (!String.IsNullOrWhiteSpace(name))
            {
                for (int i = 0; i < name.Length; i++)
                {
                    char nameChar = name[i];
                    if (!(
                        ((nameChar >= 'a') && (nameChar <= 'z')) ||
                        ((nameChar >= 'A') && (nameChar <= 'Z')) ||
                        ((nameChar >= '0') && (nameChar <= '9')) ||
                        (nameChar == '_')))
                        return true;
                }
            }
            return false;
        }

        public static string DecodeStringOrName(string value)
        {
            if (value != null)
            {
                if (value.StartsWith("'"))
                    value = ALSyntaxHelper.DecodeString(value);
                else if (value.StartsWith("\""))
                    value = ALSyntaxHelper.DecodeName(value);
            }
            return value;
        }

        public static string DecodeString(string value)
        {
            if (value != null)
            {
                value = value.Trim();
                if (value.StartsWith("'"))
                {
                    value = value.Substring(1);
                    if (value.EndsWith("'"))
                        value = value.Substring(0, value.Length - 1);
                    value = value.Replace("''", "'");
                }
                return value;
            }
            return "";
        }

        public static bool IsArrayOfStrings(string value)
        {
            if (value.StartsWith("'"))
            {
                bool inString = false;
                for (int i=0; i<value.Length; i++)
                {
                    if (value[i] == '\'')
                        inString = !inString;
                    else if ((value[i] == ',') && (!inString))
                        return true;
                }    
            }
            return false;
        }

        public static string DecodeName(string name)
        {
            if (name != null)
            {
                name = name.Trim();
                if ((name.StartsWith("\"")) && (!IsArrayOfNames(name)))
                {
                    name = name.Substring(1);
                    if (name.EndsWith("\""))
                        name = name.Substring(0, name.Length - 1);
                    name = name.Replace("\"\"", "\"");
                }
                return name;
            }
            return "";
        }

        public static bool IsArrayOfNames(string value)
        {
            if (value.StartsWith("\""))
            {
                bool inName = false;
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] == '"')
                        inName = !inName;
                    else if ((value[i] == ',') && (!inName))
                        return true;
                }
            }
            return false;
        }


        public static string EncodeName(string name)
        {
            if (NameNeedsEcoding(name))
                return "\"" + name.Replace("\"", "\"\"") + "\"";
            return name;
        }

        public static string EncodeName(string name, bool force)
        {
            if (force || NameNeedsEcoding(name))
                return "\"" + name.Replace("\"", "\"\"") + "\"";
            return name;
        }

        public static string EncodeNamesList(string[] names)
        {
            if ((names != null) && (names.Length > 0))
            {
                string list = names[0];
                for (int i = 1; i < names.Length; i++)
                {
                    list = list + ", " + EncodeName(names[i]);
                }
                return list;
            }
            return "";
        }


        private static readonly Regex InvalidObjectVariableNameRegEx = new Regex("[^a-zA-Z0-9]", RegexOptions.Compiled);

        public static string ObjectNameToVariableNamePart(string name)
        {
            return InvalidObjectVariableNameRegEx.Replace(name, String.Empty);
        }

        public static ALSymbolKind MemberAttributeToMethodKind(string name)
        {
            //events
            if (name.Equals("IntegrationEvent", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.IntegrationEventDeclaration;
            if (name.Equals("BusinessEvent", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.BusinessEventDeclaration;
            if (name.Equals("EventSubscriber", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.EventSubscriberDeclaration;
            //tests
            if (name.Equals("Test", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.TestDeclaration;
            if (name.Equals("ConfirmHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.ConfirmHandlerDeclaration;
            if (name.Equals("FilterPageHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.FilterPageHandlerDeclaration;
            if (name.Equals("HyperlinkHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.HyperlinkHandlerDeclaration;
            if (name.Equals("MessageHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.MessageHandlerDeclaration;
            if (name.Equals("ModalPageHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.ModalPageHandlerDeclaration;
            if (name.Equals("PageHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.PageHandlerDeclaration;
            if (name.Equals("ReportHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.ReportHandlerDeclaration;
            if (name.Equals("RequestPageHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.RequestPageHandlerDeclaration;
            if (name.Equals("SendNotificationHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.SendNotificationHandlerDeclaration;
            if (name.Equals("SessionSettingsHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.SessionSettingsHandlerDeclaration;
            if (name.Equals("StrMenuHandler", StringComparison.CurrentCultureIgnoreCase))
                return ALSymbolKind.StrMenuHandlerDeclaration;

            return ALSymbolKind.Undefined;
        }

        public static string RemoveComments(string source)
        {
            source = source.Trim();
            int commentPos = source.IndexOfFirst(0, "//", "/*");
            while (commentPos >= 0)
            {
                int endPos = -1;
                if (source.Substring(commentPos, 2) == "/*")
                    endPos = source.IndexOf("*/", commentPos + 2);
                else
                {
                    endPos = source.IndexOfFirst(commentPos + 2, "\n", "\r");
                    if ((endPos >= 0) && (source.Substring(endPos, 2) == "\r\n"))
                        endPos++;
                }

                if ((endPos < 0) || (endPos >= (source.Length - 1)))
                {
                    source = source.Substring(0, commentPos);
                    commentPos = -1;
                }
                else
                {
                    source = source.Substring(0, commentPos) + source.Substring(endPos + 1);
                    commentPos = source.IndexOfFirst(commentPos, "//", "/*");
                }
            }
            return source;
        }

        public static ALMemberAccessExpression DecodeMemberAccessExpression(string expression)
        {
            if (String.IsNullOrWhiteSpace(expression))
                return new ALMemberAccessExpression("", null);

            expression = expression.Trim();
            int pos = FindMemberAccessSeparator(expression);
            if (pos < 0)
                return new ALMemberAccessExpression(DecodeName(expression), null);

            if (pos == (expression.Length - 1))
                return new ALMemberAccessExpression(DecodeName(expression.Substring(0, pos).Trim()), null);

            return new ALMemberAccessExpression(
                DecodeName(expression.Substring(0, pos).Trim()),
                DecodeName(expression.Substring(pos + 1).Trim()));
        }

        public static int FindMemberAccessSeparator(string expression)
        {
            bool inName = false;
            for (int i = 0; i < expression.Length; i++)
            {
                switch (expression[i])
                {
                    case '.':
                        if (!inName)
                            return i;
                        break;
                    case '"':
                        inName = !inName;
                        break;
                }
            }
            return -1;
        }

        public static string FormatSyntaxNodeName(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return name;

            string nameText = name.ToLower();
            switch (nameText)
            {
                case "addfirst": return "AddFirst";
                case "addlast": return "AddLast";
                case "addbefore": return "AddBefore";
                case "addafter": return "AddAfter";
                case "dataset": return "DataSet";
            }
            return name;
        }

        public static List<string> DecodeNamesList(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return null;

            List<string> namesList = new List<string>();
            int startPos = 0;
            bool inName = false;
            for (int endPos = 0; endPos < value.Length; endPos++)
            {
                switch (value[endPos])
                {
                    case '"':
                        inName = !inName;
                        break;
                    case ',':
                        if (!inName)
                        {
                            namesList.Add(DecodeName(value.Substring(startPos, endPos - startPos)));
                            startPos = endPos + 1;
                        }
                        break;
                }
            }
            if (startPos < value.Length)
                namesList.Add(DecodeName(value.Substring(startPos)));

            return namesList;
        }

        public static List<string> GetWords(string value, int maxNo, int maxLength)
        {
            if (String.IsNullOrWhiteSpace(value))
                return null;
            
            List<string> words = new List<string>();
            bool inName = false;
            bool inString = false;
            int startPos = 0;
            for (int endPos = 0; endPos < value.Length; endPos++)
            {
                switch (value[endPos])
                {
                    case '"':
                        if (!inString)
                            inName = !inName;
                        break;
                    case '\'':
                        if (!inName)
                            inString = !inString;
                        break;
                    case ' ':
                    case '\n':
                    case '\r':
                    case '\t':
                    case ',':
                        if ((!inName) && (!inString) && (endPos > startPos))
                        {
                            var element = value.Substring(startPos, endPos - startPos).Trim();
                            words.Add(element);
                            startPos = endPos + 1;
                        }
                        break;
                }

                if ((words.Count >= maxNo) || ((endPos - startPos) > maxLength))
                    return words;
            }
            if (startPos < value.Length)
                words.Add(value.Substring(startPos).Trim());

            return words;
        }
    }
}
