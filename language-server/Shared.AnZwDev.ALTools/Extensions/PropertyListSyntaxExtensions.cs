using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class PropertyListSyntaxExtensions
    {

        public static PropertySyntax GetPropertyEntry(this PropertyListSyntax properties, string name)
        {
            if (properties.Properties == null)
                return null;

            foreach (PropertySyntax property in properties.Properties)
            {
                if (name.Equals(property.Name.Identifier.ValueText, StringComparison.CurrentCultureIgnoreCase))
                    return property;
            }
            return null;
        }

    }
}
