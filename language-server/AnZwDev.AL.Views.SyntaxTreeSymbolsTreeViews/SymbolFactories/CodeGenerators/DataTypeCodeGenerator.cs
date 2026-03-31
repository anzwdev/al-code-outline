using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.Formatters
{
    internal class DataTypeFormatter
    {

        public static string GetCode(DataTypeSyntax dataTypeSyntax)
        {
            return dataTypeSyntax.ToString();
        }

        public static string GetCode(TypeReferenceBaseSyntax dataTypeReference)
        {
            return dataTypeReference.ToString();
        }

    }
}
