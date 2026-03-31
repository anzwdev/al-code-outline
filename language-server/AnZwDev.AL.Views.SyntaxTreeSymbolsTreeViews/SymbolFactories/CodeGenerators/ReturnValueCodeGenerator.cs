using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.Formatters
{
    internal class ReturnValueFormatter
    {

        public static string GetCode(ReturnValueSyntax node)
        {
            return node.ToString();
        }

    }
}
