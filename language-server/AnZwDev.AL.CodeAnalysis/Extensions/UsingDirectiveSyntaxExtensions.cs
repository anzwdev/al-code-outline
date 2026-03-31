using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Extensions
{
    public static class UsingDirectiveSyntaxExtensions
    {

        public static HashSet<string>? GetUsingsNamespacesNames(this SyntaxList<UsingDirectiveSyntax> usings)
        {
            if (usings.Count == 0)
                return null;

            HashSet<string> namespacesNamesCollection = new HashSet<string>();
            for (int i = 0; i < usings.Count; i++)
            {
                var namespaceName = usings[i].Name?.ToString();
                if (!string.IsNullOrWhiteSpace(namespaceName) && !namespacesNamesCollection.Contains(namespaceName))
                    namespacesNamesCollection.Add(namespaceName);
            }

            return namespacesNamesCollection;
        }


    }
}
