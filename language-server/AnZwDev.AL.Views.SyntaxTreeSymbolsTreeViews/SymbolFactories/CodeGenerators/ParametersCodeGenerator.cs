using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.Formatters
{
    internal class ParametersFormatter
    {

        public static string GetCode(ParameterListSyntax syntax)
        {
            string namePart = "(";
            if ((syntax != null))
                namePart = namePart + syntax.Parameters.ToString();
            namePart = namePart + ")";
            return namePart;
        }

    }
}
