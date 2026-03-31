using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Symbols.Providers;
using AnZwDev.AL.Symbols.Providers.SourceCode;
using AnZwDev.AL.Symbols.Providers.AppPackages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Workspaces.Symbols
{
    public partial class ProjectSymbolsProvider
    {

        public ChangeTrackingSourceCodeSymbolsProvider ProjectCodeSymbolsProvider { get; }
        public List<SymbolsProvider> DependencySymbolsProviders { get; } = new List<SymbolsProvider>();

        public ProjectSymbolsProvider(Project project)
        {
            ProjectCodeSymbolsProvider = new ChangeTrackingSourceCodeSymbolsProvider(new ProjectSourceCodeProvider(project));
        }

        public IEnumerable<ApplicationSymbolDescriptor> GetSymbols(HashSet<string>? appIdFilter = null)
        {
            var mainSymbols = ProjectCodeSymbolsProvider.GetSymbols();

            if (mainSymbols != null)
                yield return new ApplicationSymbolDescriptor()
                {
                    Symbol = mainSymbols,
                    AccessLevelFilter = AccessLevelFilter.All
                };

            for (int i = 0; i < DependencySymbolsProviders.Count; i++)
            {
                if ((appIdFilter == null) || ((DependencySymbolsProviders[i].AppId != null) && (appIdFilter.Contains(DependencySymbolsProviders[i].AppId!))))
                {
                    var symbols = DependencySymbolsProviders[i].GetSymbols();

                    if (symbols != null)
                    {
                        var accessLevel = AccessLevelFilter.Public;
                        if ((!String.IsNullOrWhiteSpace(mainSymbols?.AppId)) && (symbols.Metadata.InternalsVisibleToModules.ContainsKey(mainSymbols.AppId)))
                            accessLevel = AccessLevelFilter.Internal;

                        yield return new ApplicationSymbolDescriptor()
                        {
                            Symbol = symbols,
                            AccessLevelFilter = accessLevel
                        };
                    }
                }
            }
        }

        public ProjectDefinitionSymbol? CreateProjectDefinitionSymbol()
        {
            var applicationSymbol = ProjectCodeSymbolsProvider.GetSymbols();
            if (applicationSymbol == null)
                return null;

            var symbol = new ProjectDefinitionSymbol()
            {
                Application = applicationSymbol
            };

            for (int i = 0; i < DependencySymbolsProviders.Count; i++)
            {
                var dependencySymbol = DependencySymbolsProviders[i].GetSymbols();
                if (dependencySymbol != null)
                    symbol.Dependencies.Add(dependencySymbol);
            }

            return symbol;
        }

    }
}
