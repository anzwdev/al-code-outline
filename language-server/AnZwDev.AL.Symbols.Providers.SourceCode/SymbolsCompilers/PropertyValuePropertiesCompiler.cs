using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    public static class PropertyValuePropertiesCompiler
    {

        public static Dictionary<string, string>? Compile(CommaSeparatedIdentifierEqualsLiteralListSyntax? syntax)
        {
            if ((syntax == null) || (syntax.Values.Count == 0))
                return null;
            
            Dictionary<string, string> properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < syntax.Values.Count; i++)
            {
                var propertySyntax = syntax.Values[i];
                var propertyName = propertySyntax.Identifier.ValueText;
                var propertyValue = propertySyntax.Literal?.ToString();
                if ((!String.IsNullOrWhiteSpace(propertyName) && (!properties.ContainsKey(propertyName))))
                    properties.Add(propertyName, propertyValue ?? String.Empty);
            }

            return properties;
        }

    }
}
