using AnZwDev.System.Extensions;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Parsing;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.AL.Syntax.Parser
{
    internal class ObjectReferenceParser
    {

        public ObjectReference Parse(ObjectKind objectKind, string? value, HashSet<string>? usings)
        {
            (var appId, var fullyQualifiedNameText) = RemoveAppIdFromFullyQualifiedName(value);

            if (Int32.TryParse(fullyQualifiedNameText, out int objectId))
                return new ObjectReference(objectKind, appId, objectId, usings);

            return new ObjectReference(objectKind, appId, ALSymbolExpressionParser.ParseFullyQualifiedName(fullyQualifiedNameText), usings);
        }

        private (string?, string?) RemoveAppIdFromFullyQualifiedName(string? fullyQualifiedName)
        {
            if ((!String.IsNullOrWhiteSpace(fullyQualifiedName)) && (fullyQualifiedName.StartsWith(ALLanguageFacts.NameAppIdDelimiter)))
            {
                int pos = fullyQualifiedName.IndexOf(ALLanguageFacts.NameAppIdDelimiter, 1);
                if (pos > 0)
                    return (fullyQualifiedName.Substring(1, pos - 1).Trim(), fullyQualifiedName.Substring(pos + 1).Trim());
            }
            return (null, fullyQualifiedName);
        }

        public List<ObjectReference>? ParseOrNull(ObjectKind objectKind, List<string>? values, HashSet<string>? usings)
        {
            if ((values == null) || (values.Count == 0))
                return null;

            List<ObjectReference> list = new List<ObjectReference>();
            for (var i = 0; i < values.Count; i++)
            {
                list.Add(this.Parse(objectKind, values[i], usings));
            }

            return list;
        }

        public List<ObjectReference>? ParseSeparatedList(ObjectKind objectKind, string? values, HashSet<string>? usings, char separator)
        {
            var referencesStringsList = values.SplitWithDelimiters(ALLanguageFacts.NameDelimiterChar, separator, true, false);
            return ParseOrNull(objectKind, referencesStringsList, usings);
        }



    }
}
