using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Metadata;
using AnZwDev.AL.Symbols.Providers;
using AnZwDev.AL.Symbols.Providers.AppPackages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Workspaces
{
    public class DependencyResolver
    {

        public Workspace Workspace { get; }

        public DependencyResolver(Workspace workspace)
        {
            this.Workspace = workspace;
        }

        public void Resolve()
        {
            for (int i = 0; i < Workspace.Projects.Count; i++)
                Resolve(Workspace.Projects[i]);
        }

        public void Resolve(Project project)
        {
            project.SymbolsProvider.DependencySymbolsProviders.Clear();

            var symbols = project.SymbolsProvider.ProjectCodeSymbolsProvider.GetSymbols();
            if (symbols?.Metadata?.Dependencies != null)
            {               
                var dependenciesList = symbols.Metadata.Dependencies;

                var packagesFullPath = project.GetPackagesCacheFullPath();
                var availableApps = AppPackageSymbolsProvidersLoader.LoadFromFolder(packagesFullPath, Workspace.SymbolsCache);

                var dependencyIndex = 0;
                while (dependencyIndex < dependenciesList.Count)
                {
                    var dependency = dependenciesList[dependencyIndex];
                    var selfDependency = (!String.IsNullOrWhiteSpace(dependency.Id)) && (dependency.Id.Equals(symbols.AppId, StringComparison.OrdinalIgnoreCase));
                    if (!selfDependency)
                    {
                        var provider = FindProvider(availableApps, dependency);
                        if (provider != null)
                        {
                            project.SymbolsProvider.DependencySymbolsProviders.Add(provider);
                            AddPropagatedDependencies(dependenciesList, provider);
                        }
                    }

                    dependencyIndex++;
                }
            }
        }

        private void AddPropagatedDependencies(List<ApplicationDependency> dependenciesList, SymbolsProvider provider)
        {
            var symbols = provider.GetSymbols();
            if ((symbols != null) && (symbols.Metadata.PropagateDependencies))
                for (int i = 0; i < symbols.Metadata.Dependencies.Count; i++)
                    AddPropagatedDependency(dependenciesList, symbols.Metadata.Dependencies[i]);
        }

        private void AddPropagatedDependency(List<ApplicationDependency> dependenciesList, ApplicationDependency propagatedDependency)
        {
            var existingDependency = FindDependency(dependenciesList, propagatedDependency);
            if (existingDependency == null)
                dependenciesList.Add(propagatedDependency);
        }

        private ApplicationDependency? FindDependency(List<ApplicationDependency> dependenciesList, ApplicationDependency propagatedDependency)
        {
            if (!String.IsNullOrWhiteSpace(propagatedDependency.Id))
                for (int i=0; i < dependenciesList.Count; i++)
                    if (propagatedDependency.Id.Equals(dependenciesList[i].Id, StringComparison.OrdinalIgnoreCase))
                        return dependenciesList[i];

            if ((SymbolsFacts.IsMicrosoftApp(propagatedDependency.Id, propagatedDependency.Name, propagatedDependency.Publisher)) && (!String.IsNullOrWhiteSpace(propagatedDependency.Name)))
                for (int i = 0; i < dependenciesList.Count; i++)
                    if (
                        (propagatedDependency.Publisher.Equals(dependenciesList[i].Publisher, StringComparison.OrdinalIgnoreCase)) &&
                        (propagatedDependency.Name.Equals(dependenciesList[i].Name, StringComparison.OrdinalIgnoreCase))
                    )
                        return dependenciesList[i];

            return null;
        }


        private SymbolsProvider? FindProvider(Dictionary<string, AppPackageSymbolsProvider> availableApps, ApplicationDependency dependency)
        {
            var dependentProject = Workspace.Projects.FindById(dependency.Id);
            if (dependentProject != null)
                return dependentProject.SymbolsProvider.ProjectCodeSymbolsProvider;

            var provider = FindProvider(availableApps, dependency, dependency.Id);
            if (provider != null)
                return provider;

            if (SymbolsFacts.IsMicrosoftApp(dependency.Id, dependency.Name, dependency.Publisher))
            {
                var altId = SymbolsFacts.GetMicrosoftAppAltId(dependency.Id, dependency.Name, dependency.Publisher);
                return FindProvider(availableApps, dependency, altId);
            }

            return null;
        }

        private AppPackageSymbolsProvider? FindProvider(Dictionary<string, AppPackageSymbolsProvider> availableApps, ApplicationDependency dependency, string? id)
        {
            if (String.IsNullOrWhiteSpace(id))
                return null;

            if (availableApps.ContainsKey(id))
            {
                var provider = availableApps[id];
                provider.Load(false);
                return provider;
            }

            return null;
        }


    }
}
