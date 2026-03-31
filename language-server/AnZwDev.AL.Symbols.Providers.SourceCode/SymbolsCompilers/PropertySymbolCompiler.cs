using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class PropertySymbolCompiler
    {

        public static PropertySymbolsCollection Compile(PropertyListSyntax? propertyList)
        {
            var properties = new PropertySymbolsCollection();
            if (propertyList != null)
            {
                foreach (var property in propertyList.Properties)
                {
                    var propertySyntax = property as PropertySyntax;
                    if (propertySyntax != null)
                    {
                        var name = ALLiteralParser.ParseName(propertySyntax.Name?.Identifier.Text);                        
                        if (!String.IsNullOrWhiteSpace(name))
                            PropertyValueCompiler.Compile(propertySyntax.Value, name, properties);                            
                    }
                }
            }
            return properties;
        }

    }
}
