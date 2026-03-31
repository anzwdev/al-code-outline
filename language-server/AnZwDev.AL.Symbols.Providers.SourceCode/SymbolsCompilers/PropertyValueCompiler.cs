using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Symbols.Parsing;
using AnZwDev.AL.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class PropertyValueCompiler
    {

        public static void Compile(PropertyValueSyntax syntax, string name, PropertySymbolsCollection propertySymbols)
        {
            switch (syntax)
            {
                case LabelPropertyValueSyntax labelValueSyntax:
                    CompileLabelPropertyValue(labelValueSyntax, name, propertySymbols);
                    break;
                default:
                    ALSymbolExpressionParser.ParsePropertyValue(propertySymbols, name, syntax.ToString());
                    break;
            }
        }

        public static void CompileLabelPropertyValue(LabelPropertyValueSyntax syntax, string name, PropertySymbolsCollection propertySymbols)
        {
            var labelSyntax = syntax.Value;
            if (labelSyntax == null)
                ALSymbolExpressionParser.ParsePropertyValue(propertySymbols, name, syntax.ToString());
            else
            {
                var value = labelSyntax.LabelText?.Value.ValueText;
                var labelProperties = PropertyValuePropertiesCompiler.Compile(labelSyntax.Properties);
                ALSymbolExpressionParser.ParsePropertyValue(propertySymbols, name, value, labelProperties);
            }
        }


    }
}
