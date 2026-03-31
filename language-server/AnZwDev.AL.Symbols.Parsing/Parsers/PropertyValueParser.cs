using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Parsing.Parsers
{
    internal abstract class PropertyValueParser
    {

        public PropertyKind Kind { get; }

        public PropertyValueParser(PropertyKind kind)
        {
            this.Kind = kind;
        }

        public abstract void Parse(PropertySymbolsCollection properties, string? value, Dictionary<string, string>? valueProperties);

    }

    internal abstract class PropertyValueParser<T> : PropertyValueParser
    {

        public PropertyValueParser(PropertyKind kind) :
            base(kind)
        {
        }

        public override void Parse(PropertySymbolsCollection properties, string? value, Dictionary<string, string>? valueProperties)
        {
            var propertyValue = ParseValue(value, valueProperties);
            properties.SetValue<T>(this.Kind, propertyValue);
        }

        protected abstract T ParseValue(string? value, Dictionary<string, string>? valueProperties);

    }

}
