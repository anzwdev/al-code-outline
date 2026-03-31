using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.Formatters
{
    internal class FieldListFormatter
    {

        public static string GetCode(SeparatedSyntaxList<IdentifierNameSyntax> fields)
        {
            return fields.ToString();
        }

    }
}
