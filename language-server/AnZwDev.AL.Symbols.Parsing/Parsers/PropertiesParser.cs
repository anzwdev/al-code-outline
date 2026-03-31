using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Parsing.Parsers
{
    internal class PropertiesParser
    {

        private Dictionary<PropertyKind, PropertyValueParser> _propertyParsers;

        public PropertiesParser()
        {
            _propertyParsers = new Dictionary<PropertyKind, PropertyValueParser>();

            AddPropertyParser(new FuncPropertyValueParser<AccessLevel>(PropertyKind.Access, (value, valueProperties) => ALSymbolExpressionParser.ParseAccessLevel(value)));
            AddPropertyParser(new FuncPropertyValueParser<Label>(PropertyKind.Caption, (value, valueProperties) => ALSymbolExpressionParser.ParseLabel(value, valueProperties)));
            AddPropertyParser(new FuncPropertyValueParser<string?>(PropertyKind.Description, (value, valueProperties) => value));
            AddPropertyParser(new FuncPropertyValueParser<Label>(PropertyKind.ToolTip, (value, valueProperties) => ALSymbolExpressionParser.ParseLabel(value, valueProperties)));

            AddPropertyParser(new FuncPropertyValueParser<bool>(PropertyKind.Enabled, (value, valueProperties) => ALLiteralParser.ParseBool(value, true)));
            AddPropertyParser(new FuncPropertyValueParser<FieldClass>(PropertyKind.FieldClass, (value, valueProperties) => ALSymbolExpressionParser.ParseFieldClass(value)));
            AddPropertyParser(new FuncPropertyValueParser<ObsoleteState>(PropertyKind.ObsoleteState, (value, valueProperties) => ALSymbolExpressionParser.ParseObsoleteState(value)));

            AddPropertyParser(new FuncPropertyValueParser<string?>(PropertyKind.ObsoleteReason, (value, valueProperties) => value));
            AddPropertyParser(new FuncPropertyValueParser<string?>(PropertyKind.SourceExpression, (value, valueProperties) => value));
            AddPropertyParser(new FuncPropertyValueParser<string?>(PropertyKind.SourceTable, (value, valueProperties) => value));
            AddPropertyParser(new FuncPropertyValueParser<string?>(PropertyKind.IncludedPermissionSets, (value, valueProperties) => value));
            AddPropertyParser(new FuncPropertyValueParser<string?>(PropertyKind.ExcludedPermissionSets, (value, valueProperties) => value));
            AddPropertyParser(new FuncPropertyValueParser<string?>(PropertyKind.InherentPermissions, (value, valueProperties) => value));
        }

        private void AddPropertyParser(PropertyValueParser parser)
        {
            _propertyParsers.Add(parser.Kind, parser);
        }

        public void Parse(PropertySymbolsCollection properties, string? name, string? value, Dictionary<string, string>? valueProperties)
        {
            if (!String.IsNullOrWhiteSpace(name))
            {
                var propertyKind = ALSymbolExpressionParser.ParsePropertyKind(name);
                if (_propertyParsers.ContainsKey(propertyKind))
                    _propertyParsers[propertyKind].Parse(properties, value, valueProperties);
            }
        }

    }
}
