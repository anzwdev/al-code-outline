using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Extensions
{
    public static class PropertyListSyntaxExtensions
    {

        public static PropertySyntax? GetPropertyEntry(this PropertyListSyntax properties, string name)
        {
            foreach (PropertySyntax property in properties.Properties)
                if (name.Equals(property.Name.Identifier.ValueText, StringComparison.OrdinalIgnoreCase))
                    return property;
            return null;
        }

    }
}
