using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class MethodDeclarationSyntaxExtensions
    {

        public static bool IsEventSubscriber(this MethodDeclarationSyntax syntax)
        {
            return
                (syntax.Attributes != null) &&
                (syntax.Attributes.Where(p => (p.Name != null) && (p.Name.ToString().Equals("EventSubscriber", StringComparison.CurrentCultureIgnoreCase))).Any());
        }


    }
}
