using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Extensions
{
    public static class MethodDeclarationSyntaxExtensions
    {

        public static bool IsEventSubscriber(this MethodDeclarationSyntax syntax)
        {
            return
                (syntax.Attributes.Where(p => (p.Name != null) && (p.Name.ToString().Equals("EventSubscriber", StringComparison.OrdinalIgnoreCase))).Any());
        }

    }
}
