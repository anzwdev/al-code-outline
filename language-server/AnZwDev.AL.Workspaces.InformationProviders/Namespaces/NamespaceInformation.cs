using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.InformationProviders.Namespaces
{
    public class NamespaceInformation
    {

        public required string? Namespace { get; init; }
        public required HashSet<string>? Usings { get; init; }

    }
}
