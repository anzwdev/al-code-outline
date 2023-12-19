using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{

#if BC

    public static class SyntaxList_UsingsExtensions
    {

        public static HashSet<string> GetUsingsNamespacesNames(this SyntaxList<UsingDirectiveSyntax> usings)
        {
            if ((usings == null) || (usings.Count == 0))
                return null;
            
            HashSet<string> namespacesNamesCollection = new HashSet<string>();
            for (int i=0; i<usings.Count; i++)
            {
                var namespaceName = usings[i].Name?.ToString();
                if ((!String.IsNullOrWhiteSpace(namespaceName)) && (!namespacesNamesCollection.Contains(namespaceName)))
                    namespacesNamesCollection.Add(namespaceName);
            }

            return namespacesNamesCollection;
        }

    }

#endif

}
