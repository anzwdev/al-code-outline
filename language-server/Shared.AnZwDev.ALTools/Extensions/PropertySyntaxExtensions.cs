using AnZwDev.ALTools.CodeTransformations;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class PropertySyntaxExtensions
    {

        public static PropertySyntax SortCommaSeparatedPropertyValue(this PropertySyntax property, out bool sorted)
        {
            sorted = false;
            CommaSeparatedPropertyValueSyntax value = property.Value as CommaSeparatedPropertyValueSyntax;
            if (value != null)
            {
                value = value.WithValues(
                    SyntaxNodesGroupsTree<IdentifierNameSyntax>.SortSeparatedSyntaxList(value.Values, new IdentifierNameComparer(), out sorted));
                property = property.WithValue(value);
            }
            return property;
        }

        public static bool IsNullOrEquals(this PropertySyntax propertySyntax, string value, StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            var propertyValueText = propertySyntax?.Value?.ToString();
            return (propertyValueText == null) || (propertyValueText.Equals(value, stringComparison));
        }

        public static bool IsNotNullAndEquals(this PropertySyntax propertySyntax, string value, StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            var propertyValueText = propertySyntax?.Value?.ToString();
            return (propertyValueText != null) && (propertyValueText.Equals(value, stringComparison));
        }


    }
}
