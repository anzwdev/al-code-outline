using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class PageGroupSyntaxExtensions
    {

        public static bool IsRepeater(this PageGroupSyntax node)
        {
            string nodeType = node.ControlKeyword.Text;
            return
                (nodeType != null) &&
                (nodeType.Equals("repeater", StringComparison.CurrentCultureIgnoreCase));
        }

    }
}
