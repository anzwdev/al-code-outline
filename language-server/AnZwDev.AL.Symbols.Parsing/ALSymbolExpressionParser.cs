using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Symbols.Parsing.Parsers;
using AnZwDev.AL.Syntax;
using AnZwDev.AL.Syntax.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Parsing
{
    public static class ALSymbolExpressionParser
    {

        private static readonly FullyQualifiedNameParser _fullyQualifiedNameParser = new FullyQualifiedNameParser();
        private static readonly ObjectReferenceParser _objectReferenceParser = new ObjectReferenceParser();
        private static readonly LabelParser _labelParser = new LabelParser();

        private static readonly ALEnumParser<PropertyKind> _propertyKindParser = new ALEnumParser<PropertyKind>(PropertyKind.Undefined);
        private static readonly ALEnumParser<AccessLevel> _accessLevelParser = new ALEnumParser<AccessLevel>(AccessLevel.Public);
        private static readonly ALEnumParser<FieldClass> _fieldClassParser = new ALEnumParser<FieldClass>(FieldClass.Normal);
        private static readonly ALEnumParser<ObsoleteState> _obsoleteStateParser = new ALEnumParser<ObsoleteState>(ObsoleteState.No);
        private static readonly ObjectKindParser _objectKindParser = new ObjectKindParser();
        private static readonly MemberKindParser _memberKindParser = new MemberKindParser();

        private static readonly PropertiesParser _propertiesParser = new PropertiesParser();
        private static readonly VersionParser _versionParser = new VersionParser();

        private static readonly TableFieldExpressionReferenceParser _tableFieldExpressionReferenceParser = new TableFieldExpressionReferenceParser();

        public static FullyQualifiedName ParseFullyQualifiedName(string? value)
        {
            return _fullyQualifiedNameParser.Parse(value);
        }

        public static ObjectReference ParseObjectReference(ObjectKind kind, string? value, HashSet<string>? usings)
        {
            return _objectReferenceParser.Parse(kind, value, usings);
        }

        public static List<ObjectReference>? ParseObjectReferenceListOrNull(ObjectKind kind, List<string>? value, HashSet<string>? usings)
        {
            return _objectReferenceParser.ParseOrNull(kind, value, usings);
        }

        public static List<ObjectReference>? ParseObjectReferenceSeparatedListOrNull(ObjectKind kind, string? values, HashSet<string>? usings, char separator = ',')
        {
            return _objectReferenceParser.ParseSeparatedList(kind, values, usings, separator);
        }

        public static Label ParseLabel(string? value, Dictionary<string, string>? valueProperties)
        {
            return _labelParser.Parse(value, valueProperties);
        }

        public static PropertyKind ParsePropertyKind(string? value)
        {
            return _propertyKindParser.Parse(value);
        }

        public static AccessLevel ParseAccessLevel(string? value)
        {
            return _accessLevelParser.Parse(value);
        }

        public static FieldClass ParseFieldClass(string? value)
        {
            return _fieldClassParser.Parse(value);
        }

        public static ObsoleteState ParseObsoleteState(string? value)
        {
            return _obsoleteStateParser.Parse(value);
        }

        public static ObjectKind ParseObjectKind(string? value)
        {
            return _objectKindParser.Parse(value);
        }

        public static MemberKind ParseMemberKind(string? value)
        {
            return _memberKindParser.Parse(value);
        }

        public static void ParsePropertyValue(PropertySymbolsCollection properties, string? name, string? value, Dictionary<string, string>? valueProperties = null)
        {
            _propertiesParser.Parse(properties, name, value, valueProperties);
        }

        public static Version ParseVersion(string? value, int major, int minor, int build, int revision)
        {
            return _versionParser.Parse(value, major, minor, build, revision);
        }

        public static Version ParseVersion(string? value, int major, int minor)
        {
            return _versionParser.Parse(value, major, minor);
        }


        public static string? ParseTableFieldExpressionReference(string? expression)
        {
            return _tableFieldExpressionReferenceParser.Parse(expression);
        }


    }
}
