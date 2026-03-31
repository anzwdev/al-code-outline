using AnZwDev.AL.Symbols.Platform;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.InformationProviders.Namespaces
{
    public static class NamespaceInformationProvider
    {

        public static NamespacesState GetNamespacesState(Project project)
        {
            var projectSymbols = project.SymbolsProvider.ProjectCodeSymbolsProvider.GetSymbols();
            if (projectSymbols == null)
                return NamespacesState.NotSupported;

            var projectCapabilities = new PlatformCapabilities(projectSymbols.Metadata.BCRuntimeVersion);
            if (!projectCapabilities.Namespaces)
                return NamespacesState.NotSupported;

            if (projectSymbols.AllObjects.UsesNamespaces())
                return NamespacesState.Recommended;

            return NamespacesState.Supported;
        }

    }
}
